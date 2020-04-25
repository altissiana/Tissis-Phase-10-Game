using System.Collections.Generic;

namespace server.Models.Dtos
{
    public class PhaseResourceModel
    {
        public string PlayerId { get; set; }
        public int PhaseType{ get; set; }
        public List<CardCombinationResourceModel> CardCombinations { get; set; }
    }
}