using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Medium_Assignment.API.Controllers
{
    public class TestGetViewModel
    {

        public int value1 { get; set; }

        public int value2 { get; set; }

    }

    public class TestPostViewModel
    {

        public int value1 { get; set; }

        public int value2 { get; set; }

    }

    public class TestPutViewModel
    {
        public int value1 { get; set; }

        public int value2 { get; set; }

    }

   //[Authorize]
    public class TestController : ApiController
    {
        // GET api/test
        public IHttpActionResult Get()
        {
            var model = new TestGetViewModel {
                value1 = 1,
                value2 = 2
            };

            
            return Ok(model);
        }

        public IHttpActionResult Get(int id)
        {
            var model = new TestGetViewModel
            {
                value1 = 3,
                value2 = 4
            };


            return Ok(model);
        }

        public IHttpActionResult Post(TestPostViewModel model)
        {
           
            return Ok();
        
        }

        public IHttpActionResult Put(int id, TestPostViewModel model)
        {

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {

            return Ok();
        }

    }
}
