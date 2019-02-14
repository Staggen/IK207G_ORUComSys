using Datalayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datalayer.Repositories {
    public class AttachmentRepository : Repository<AttachmentModels, int> {
        public AttachmentRepository(ApplicationDbContext context) : base(context) { }

        public List<AttachmentModels> GetAttachmentsByPostId(int postId) {
            return items.Where((a) => a.PostId.Equals(postId)).ToList();
        }

        public byte[] GetAttachmentByteArrayById(int attachmentId) {
            return items.Single((a) => a.Id.Equals(attachmentId)).AttachedFile;
        }
    }
}