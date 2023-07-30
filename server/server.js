const http = require('http');
const finalhandler = require('finalhandler');
const serveStatic = require('serve-static');

// This script is run as "node server.js path/to/distribution"
// Take path to files to serve from command line arg 2
const distribution_path = process.argv[2];

const serve = serveStatic(
  distribution_path,
  {
    index: ['index.html'],
  }
);

const server = http.createServer((req, res) => {
  const done = finalhandler(req, res);
  serve(req, res, done);
});

const port = process.env.PORT || 8080;
server.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});
