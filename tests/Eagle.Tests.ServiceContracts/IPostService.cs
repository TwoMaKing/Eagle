using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain.Models;

namespace Eagle.Tests.ServiceContracts
{
    [ServiceContract()]
    public interface IPostService
    {
        [OperationContract()]
        void PublishPost(PostDataObject post);

        [OperationContract()]
        [WebGet(UriTemplate = "Posts/All", ResponseFormat=WebMessageFormat.Json)]
        IEnumerable<PostDataObject> GetPosts();
    }
}
