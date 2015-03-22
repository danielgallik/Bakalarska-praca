using System.Collections.Generic;
using System.Linq;
using Alarmee.WardManager.Contracts;
using PumpContract;

namespace Alarmee.WardManager
{
	internal class RemainingTimesEngine
	{
		public IList<RemainingTimeInfo> GetRemainingTimes(List<PumpDto> pumps)
		{
			var result = new List<RemainingTimeInfo>();

			pumps.Where(x => x.CurrentState.Equals("Running")).ToList().ForEach(x => result.Add(new RemainingTimeInfo
			{
				Medicament = x.Medicament,
				Progress = calculateRemainingTimeProgress(x),
				RemainingTime = formatRemainingTime(x)
			}));

			return result;
		}

		private int calculateRemainingTimeProgress(PumpDto pump)
		{
			return (pump.RemainingTime / pump.TotalTime) * 100;
		}

		private string formatRemainingTime(PumpDto pump)
		{
			// TODO: needs to be impl. according spec.
			return string.Format("{0} s", pump.RemainingTime);
		}
	}
}
