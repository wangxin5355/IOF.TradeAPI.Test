using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.BizCommon.Data.Entity
{
	public class SpotHisPriceEntity
	{
		public long HID { get; set; }

		public long SpotID { get; set; }

		public double HighPx { get; set; }

		public double OpenPx { get; set; }

		public double LowPx { get; set; }

		public double LastPx { get; set; }

		public double PreClosePx { get; set; }

		public int Vol { get; set; }

		public DateTime TradeDate { get; set; }

		public DateTime AddTime { get; set; }

		public DateTime UpdateTime { get; set; }
	}
}
