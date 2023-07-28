const http = require('http');
const finalhandler = require('finalhandler');
const serveStatic = require('serve-static');

const serve = serveStatic('.', {
  index: ['index.html'],
});

const server = http.createServer((req, res) => {
  const done = finalhandler(req, res);
  serve(req, res, done);
});

const port = process.env.PORT || 8080;
server.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});
