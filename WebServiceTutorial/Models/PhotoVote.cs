using System;

namespace CryptOverseeMobileApp.Models
{

    public class PhotoVote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? NbVotes { get; set; }
      
        public decimal? SumVotes { get; set; }
       
        public DateTime? Date { get; set; }
    }
}
