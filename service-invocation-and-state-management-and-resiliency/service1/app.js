const express = require("express");
const axios = require("axios");

const app = express();
const port = 3000;
const daprPort = 3500; // Dapr sidecar port
const daprUrl = `http://service1-dapr:${daprPort}/v1.0`;

app.use(express.json());

app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).send("Something broke!");
});

app.get("/save/:key/:value", async (req, res) => {
  console.log(req.params);
  const { key, value } = req.params;
  try {
    const response = await axios.post(
      `${daprUrl}/state/statestore`,
      [{ key, value }],
      {
        headers: { "Content-Type": "application/json" },
      }
    );
    res.send("State saved");
  } catch (error) {
    console.error(error);
    res.send("Error saving state");
  }
});

app.get("/get/:key", async (req, res) => {
  console.log(req.params);
  const { key } = req.params;
  try {
    const response = await axios.get(`${daprUrl}/state/statestore/${key}`);
    res.send(response.data);
  } catch (error) {
    console.error(error);
    res.send("Error getting state");
  }
});

app.get("/sayhello", (req, res) => {
  res.send("Hello from Service 1!");
});

app.listen(port, () => {
  console.log(`Service 1 listening on port ${port}`);
});
