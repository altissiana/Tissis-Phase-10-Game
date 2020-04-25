using System.Collections.Generic;

namespace server.Models.Dtos
{
    public class CardCombinationResourceModel
    {
        public int Length { get; set; }
        public List<CardResourceModel> Cards { get; set; }
        public int CardCombinationType { get; set; }
    }
}