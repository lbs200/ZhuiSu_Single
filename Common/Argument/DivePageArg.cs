
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Argument
{
    /// <summary>
    /// 分页参数实例对象
    /// </summary
    public class DivePageArg
    {
        
        /// <summary>
        /// 当前页码
        /// </summary>
        int _CurrentIndex = 1;
        /// <summary>
        /// 页记录大小
        /// </summary>
        int _PageSize = 5;
        /// <summary>
        /// 总页数
        /// </summary>
        int _PageCount = 0;
        /// <summary>
        /// 总记录
        /// </summary>
        int _TotalCount = 0;

        public DivePageArg() 
        { 
        
        }

        /// <summary>
        /// 初始化分页参数
        /// </summary>
        /// <param name="curIndex">当前索引</param>
        /// <param name="pSize">页大小</param>
        /// <param name="tCount">总条目数</param>
        /// <returns></returns>
        public DivePageArg(int curIndex, int pSize, int tCount)
        {
            this._CurrentIndex = curIndex;
            this._PageSize = pSize;
            this._TotalCount = tCount;
        }

        public static DivePageArg DivePageVase() { return new DivePageArg(); }

        /// <summary>
        /// 初始化分页参数
        /// </summary>
        /// <param name="curIndex">当前索引</param>
        /// <param name="pSize">页大小</param>
        /// <param name="tCount">总条目数</param>
        /// <returns></returns>
        public bool InitParam(int curIndex,int pSize,int tCount)
        {
            try
            {
                this._CurrentIndex = curIndex;
                this._PageSize = pSize;
                this._TotalCount = tCount;
                //this._PageCount = (int)Math.Ceiling((decimal)_TotalCount / _PageSize);
                return true;
            }
            catch
            {
                return false; 
            }
        }

        #region 属性
        /// <summary>
        /// 页记录大小
        /// </summary>
        public int PageSize 
        { 
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentIndex 
        { 
            get { return _CurrentIndex; }
            set { _CurrentIndex = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount 
        {
            get { return _PageCount = (int)Math.Ceiling((decimal)_TotalCount / _PageSize); }
        }
        /// <summary>
        /// 总记录
        /// </summary>
        public int TotalCount
        {
            get { return _TotalCount; }
            set
            {
                _TotalCount = value;
            }
        }
        #endregion
    }
}
