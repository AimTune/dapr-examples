const express = require('express');
const axios = require('axios');
const app = express();
const port = 3001;

app.get('/invoke', async (req, res) => {
    try {
        const response = await axios.get(`http://service1-dapr:3500/v1.0/invoke/service1/method/sayhello`);
        res.send(`Service 1 says: ${response.data}`);
    } catch (error) {
        res.send(`Error: ${error.message}`);
    }
});

app.listen(port, () => {
    console.log(`Service 2 listening on port ${port}`);
});
