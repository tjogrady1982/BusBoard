using BusBoard.Api;
using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
  public class BusInfo
  {
    public BusInfo(List<Arrivals> arrivals)
    {
      Arrivals = arrivals;
    }

    public List<Arrivals> Arrivals { get; set; }

  }
}