using System.Collections.Generic;

namespace FaceBLL
{
    public class FaceResut
    {
        public resultdata data { set; get; }

        public int errno { set; get; }

        public string msg { set; get; }
    }

    public class userdata
    {
        public string create_time { set; get; }
        public string face_token { set; get; }

        public string user_info { set; get; }

        public string group_id { set; get; }

        public string score { get; set; }

        public string user_id { get; set; }
    }

    public class resultdata
    {
        public string log_id { set; get; }
        public string face_token { set; get; }

        public List<userdata> result { set; get; }
        public string group_id { get; set; }

        public int result_num { set; get; } = 0;


    }
}
