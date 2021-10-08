using System;
using System.Collections.Generic;
using System.Text;

namespace DigiKalaSellerBot.Core.Model
{
    public class CustomerModel
    {
        public string Name { get; set; }

        public string Mobile { get; set; }


        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }


        public string ShipmentId { get; set; }
    }
}
