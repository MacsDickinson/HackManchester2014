using System;

namespace HackManchester2014.Domain
{
    public class Donation
    {
        public decimal? Amount { get; set; }
        public DateTime? DonationDate { get; set; }
        public int DonationId { get; set; }
        public string DonationRef { get; set; }
        public string DonorDisplayName { get; set; }
        public decimal? EstimatedTaxReclaim { get; set; }
        public bool HasImage { get; set; }
        public string Image { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }

        public static implicit operator Donation(JustGiving.Api.Sdk.Model.Donation.Donation d)
        {
            return new Donation()
            {
                Amount = d.Amount,
                DonationDate=d.DonationDate,
                DonationId=d.DonationId,
                DonationRef=d.DonationRef,
                DonorDisplayName=d.DonorDisplayName,
                EstimatedTaxReclaim=d.EstimatedTaxReclaim,
                HasImage=d.HasImage,
                Image=d.Image,
                Message=d.Message,
                Source=d.Source
            };
        }

    }
}