const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');

const app = express();
const mongoose = require('mongoose');
require('dotenv/config');
const PORT = 5000;

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.use(cors());
// app.set('view engine', 'ejs');


// connect to database
mongoose.connect(
    process.env.DB_CONNECTION,
    // "mongodb://localhost:27017/NidCreation",
    {useUnifiedTopology: true, useNewUrlParser: true },
    () => {
    console.log('Connected to Database');
});

// import routes

app.use("/uploads", express.static("uploads")); //http://localhost:9000/uploads/myphoto.jpg
const postRoute = require('./routes/post');
app.use('/posts', postRoute,);
// app.use(auth);

app.get('/',(req, res) => {
    res.send('App is running');
});


app.listen(PORT,() => {
    console.log(`app is running on http://localhost:${PORT}`);
});