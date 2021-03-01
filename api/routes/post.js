const express = require('express');
const multer = require('multer');
const router = express.Router();
const storage = multer.diskStorage({
    destination: function(req, file, cb){
        cb(null, './uploads/');
    },filename: function(req, file, cb) {
        cb(null, file.originalname);
    }
});
const upload = multer({storage: storage});

const Post = require('../models/Post');

router.route('/')
.get((req,res) => {
    Post.find((e,posts) => {
        if(e){
            res.send(e);
        } else{
            res.send(posts);
            console.log('Fetched every docs');
        }
    });
})
.post(upload.single('img'),(req,res) => {
    console.log(req.file);
    const post = new Post({
        title: req.body.title,
        description: req.body.description,
        img: req.file.path
    });
    console.log(post);
    post.save((e) => {
        if(e){
            console.log(e);
        } else{
            res.send('inserted');
            console.log('inserted');
        }
    });
})
.delete((req,res) => {
    Post.deleteMany((e) => {
        if(e){
            res.send(e);
        }else{
            res.send('Successfully Deleted!!');
        }
    });
});

///////////////////////    SPECIFIC ROUTES    /////////////////////

router.route('/:postTitle')
.get((req,res) => {
    Post.findOne({title: req.params.postTitle},(e,post) => {
        if(post){
            res.send(post);
        } else{
            res.send('No article found :(');
        }
    });
})
.put(upload.single('img'),(req,res) => {
    Post.update(
        {title: req.params.postTitle},
        {title: req.body.title, description: req.body.description, img: req.file.path},
        {overwrite: true},
        (e) => {
            if(!e){
                res.send('Update SuccessFully');
            } else{
                res.send(e);
            }
        } 
    );
})
.delete((req,res) => {
    Post.deleteOne(
        {title:req.params.postTitle},
        (e) => {
            if(!e){
                res.send('Post Deleted');
            } else{
                res.send('Record Not found');
            }
        }
    );
});

module.exports = router;