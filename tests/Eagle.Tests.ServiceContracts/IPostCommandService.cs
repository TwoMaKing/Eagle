using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain.Models;

namespace Eagle.Tests.ServiceContracts
{
    [ServiceContract()]
    public interface IPostCommandService
    {
        [OperationContract()]
        void PublishPost(PostDataObject post);
    }

}
