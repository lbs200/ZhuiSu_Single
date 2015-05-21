using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Trace_Rec : DALBase
    {
        public LinqModel.Trace_Rec Scan(string ewm)
        {
            var modelResult = new LinqModel.Trace_Rec();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Trace_Rec.FirstOrDefault(m => m.Trace_Code == ewm);
                    if (model != null && model.Trace_Rec_ID > 0)
                    {
                        model.Scan_Times += 1;
                        model.Shoot_Times += 1;
                    }
                    else
                    {
                        model = new LinqModel.Trace_Rec();
                        model.ErrorTimes = 0;
                        model.First_Verify_Time = null;
                        model.Scan_Times = 1;
                        model.Shoot_Times = 1;
                        model.State = "0";
                        model.Trace_Code = ewm;
                        dc.Trace_Rec.InsertOnSubmit(model);
                    }
                    dc.SubmitChanges();
                    modelResult = dc.Trace_Rec.FirstOrDefault(m => m.Trace_Code == ewm);
                }
            }
            catch { modelResult = new LinqModel.Trace_Rec(); }
            return modelResult;
        }

        public LinqModel.Trace_Rec Verify(string ewm, bool success)
        {
            var modelResult = new LinqModel.Trace_Rec();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Trace_Rec.FirstOrDefault(m => m.Trace_Code == ewm);
                    if (model != null && model.Trace_Rec_ID > 0)
                    {
                        model.State = "1";
                        if (model.First_Verify_Time == null)
                        {
                            model.First_Verify_Time = Common.Argument.Public.GetDateTimeNow();
                        }
                        if(!success)
                        {
                            model.ErrorTimes += 1;
                        }
                    }
                    dc.SubmitChanges();
                    modelResult = dc.Trace_Rec.FirstOrDefault(m => m.Trace_Code == ewm);
                }
            }
            catch { modelResult = new LinqModel.Trace_Rec(); }
            return modelResult;
        }
    }
}
