using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets;
using Data.Models.Chalets.ChaletDetails;

namespace Data.ViewModels
{
    public class ReservationViewModel
    {
        public ReservationViewModel()
        {
            Reservations=new List<Reservation>();
            ReservationModels=new List<ReservationModel>();
        }
        public List<ReservationModel> ReservationModels { get; set; }
        public Chalet Chalet { get; set; }
        public List<Reservation> Reservations { get; set; }
        public Unit SelectedUnit { get; set; }
        public DateTime Date { get; set; }
    }

    public class ReservationModel
    {
        public ReservationModel()
        {
            Unit = new Unit();
        }
        public Unit Unit { get; set; }
        public bool Available { get; set; }

    }
}
