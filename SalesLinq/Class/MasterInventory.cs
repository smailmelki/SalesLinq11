using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SalesLinq.Class.Master;

namespace SalesLinq.Class
{
    public static class MasterInventory
    {
        public enum CostCalculationMethod
        {
            FIFO=1,
            LIFO,
            WAVP
        }
        /// <summary>
        /// طريقة حساب تكلفة في نفس الصنف مع اختلاف سعره 
        /// </summary>
        public static List<ValueAndID> CostCalculationMethodList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)CostCalculationMethod.FIFO,Name="الوارد أولا يخرج أولا" },
                new ValueAndID() { ID=(int)CostCalculationMethod.LIFO,Name="الوارد آخرا يخرج أولا" },
                new ValueAndID() { ID=(int)CostCalculationMethod.WAVP,Name="المتوسط المرجح" },
            };
        /// <summary>
        /// حساب تكلفة الصنف المضاف للفاتورة على حسب تكلفة كل عنصر مع اختلاف سعره
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="storeID"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static double GetCostOfNextProduct(int productID,int storeID , double qty)
        {
            using (var db = new DAL.DBDataContext())
            {
                var query = db.StoreLogs.Where(sl => sl.ProductID == productID && sl.StoreID == storeID)
                    .OrderBy(sl => sl.InsertTime);

                if (qty <= 0)
                    throw new ArgumentOutOfRangeException(nameof(qty), "يجب أن تكون الكمية المطلوبة أكبر من صفر.");

                if (query.Count() == 0) // في حالة لم تكن هناك أي حركة على الصنف نضع السعر الأصلي
                {
                    var product = Session.ProductView.SingleOrDefault(x => x.ID == productID);
                    var defaultUnit = product?.Units?.OrderByDescending(x => x.Factor).FirstOrDefault();
                    if (defaultUnit == null)
                        throw new InvalidOperationException("لا يوجد سعر شراء افتراضي للصنف.");

                    return defaultUnit.BuyPrice;
                }

                double TotalQtyOut = query.Where(q => q.IsInTransaction == false).Sum(q => (double?)q.Qty) ?? 0;//مجموع الكمية المباعة
                double Balance = GetProductBalanceInStore(productID, storeID);// الرصيد الحالي
                if (Balance <= 0)
                {
                    return 0;
                }
                var subQuery = query
                    .Where(q => query.Where(q1 => q1.IsInTransaction == true && q1.InsertTime <= q.InsertTime)
                    .Sum(q1 => q1.Qty) > TotalQtyOut && q.IsInTransaction == true).ToList();
                if (subQuery.Count == 0)
                    throw new InvalidOperationException("رصيد الصنف موجب، لكن تعذر تحديد طبقات تكلفة المخزون. راجع حركات الصنف.");

                var subQueryBalance = subQuery.Sum(q => q.Qty);
                if (subQueryBalance > Balance)
                {
                    var diff = subQueryBalance - Balance;
                    if (diff > subQuery[0].Qty)
                        throw new InvalidOperationException("تعذر مطابقة طبقات المخزون مع الرصيد الحالي.");

                    subQuery[0].Qty -= diff;                // حذف الكمية الزائدة (المباعة ) من أول طبقة
                }

                subQuery = subQuery.Where(q => q.Qty > 0).ToList();
                if (subQuery.Count == 0 || subQuery.Sum(q => q.Qty) <= 0)
                    throw new InvalidOperationException("لا توجد طبقات مخزون موجبة لحساب تكلفة الصنف.");

                double fifo;
                double lifo;
                double WAC;
                ////////////////// الوارد أولا يخرج أولا - FIFO - حساب التكلفة بحسب ///////////////////
                if (subQuery[0].Qty < qty)
                {
                    int i = 0;
                    var qtyX = qty;
                    double SumPrice = 0;
                    while (qtyX > 0 && i < subQuery.Count())
                    {
                        var row = subQuery[i];
                        double qty1 = (qtyX > row.Qty) ? row.Qty : qtyX;
                        SumPrice += qty1 * row.CostValue;
                        qtyX -= qty1;
                        i++;
                    }
                    fifo = SumPrice / (qty - qtyX);
                }
                else
                {
                    fifo = subQuery.First().CostValue;
                }
                ////////////////// الوارد آخرا يخرج أولا - LIFO - حساب التكلفة بحسب ///////////////////
                subQuery = subQuery.OrderByDescending(q=>q.InsertTime).ToList();
                if (subQuery[0].Qty < qty)
                {
                    int i = 0;
                    var qtyX = qty;
                    double SumPrice = 0;
                    while (qtyX > 0 && i < subQuery.Count())
                    {
                        var row = subQuery[i];
                        double qty1 = (qtyX > row.Qty) ? row.Qty : qtyX;
                        SumPrice += qty1 * row.CostValue;
                        qtyX -= qty1;
                        i++;
                    }
                    lifo = SumPrice / (qty- qtyX);
                }
                else
                {
                    lifo = subQuery.First().CostValue;
                }
                ////////////////// المتوسط المرجح - WAC - حساب التكلفة بحسب ///////////////////
                var remainingQty = subQuery.Sum(q => q.Qty);
                if (remainingQty <= 0)
                    throw new InvalidOperationException("لا يمكن حساب المتوسط المرجح دون رصيد موجب.");

                WAC = subQuery.Sum(q => q.Qty * q.CostValue) / remainingQty;
                /////////////////////////////////////////////////////////////////////////////////
                var method = (CostCalculationMethod)Session.Products.Single(x => x.ID == productID).CostCalculationMethod;
                switch (method)
                {
                    case CostCalculationMethod.FIFO: return fifo;
                    case CostCalculationMethod.LIFO: return lifo;
                    case CostCalculationMethod.WAVP: return WAC;
                    default: return fifo;
                }
            }
        }
        /// <summary>
        /// دالة حساب الرصيد
        /// </summary>
        /// <param name="productID">كود الصنف</param>
        /// <param name="storeID">كود المخزن</param>
        /// <returns>الرصيد الحالي</returns>
        public static double GetProductBalanceInStore(int productID, int storeID)
        {
            using (var db = new DAL.DBDataContext())
            {
                var query = db.StoreLogs.Where(sl => sl.ProductID == productID && sl.StoreID == storeID);
                double TotalQtyOut = query.Where(q => q.IsInTransaction == false).Sum(q => (double?)q.Qty) ?? 0;
                double TotalQtyIn = query.Where(q => q.IsInTransaction == true).Sum(q => (double?)q.Qty) ?? 0;
                double Balance = TotalQtyIn - TotalQtyOut;
                return Balance;
            }
        }
    }
}
