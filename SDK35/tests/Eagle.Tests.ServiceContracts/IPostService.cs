using Eagle.Domain.Application;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Eagle.Tests.ServiceContracts
{
    [ServiceContract()]
    public interface IPostService : IApplicationServiceContract
    {
        [OperationContract()]
        void PublishPost(PostDataObject post);

        [OperationContract()]
        [WebGet(UriTemplate = "Posts/All", ResponseFormat=WebMessageFormat.Json)]
        IEnumerable<PostDataObject> GetPosts();
    }
}
