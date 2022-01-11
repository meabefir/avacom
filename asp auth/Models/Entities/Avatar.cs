using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class Avatar
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HairId { get; set; }
        public int EyesId { get; set; }
        public int NoseId { get; set; }
        public int FaceId { get; set; }
        public int LipsId { get; set; }
        public int AccessoryId{ get; set; }
        public int ClothingId{ get; set; }

        public User User { get; set; }
    }
}
