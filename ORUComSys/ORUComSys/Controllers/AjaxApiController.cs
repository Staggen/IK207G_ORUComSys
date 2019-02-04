﻿using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Http;

namespace ORUComSys.Controllers {
    [Authorize]
    public class AjaxApiController : ApiController {
        private PostRepository postRepository;

        public AjaxApiController() {
            ApplicationDbContext context = new ApplicationDbContext();
            postRepository = new PostRepository(context);
        }

        [HttpPost]
        public void AddPost(PostModels post) {
            if (ModelState.IsValid) {

                PostModels postModel = new PostModels() {
                    Content = post.Content,
                    PostFromId = User.Identity.GetUserId(),
                    PostDateTime = DateTime.Now,
                    Forum = post.Forum,
                    CategoryId = 1
                };

                postRepository.Add(postModel);
                postRepository.Save();
            }
        }

        [HttpDelete]
        public void DeletePost(int id) {
            postRepository.Remove(id);
            postRepository.Save();
        }
    }
}