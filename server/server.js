const fs = require('fs');
const https = require('https');
const compression = require('compression');
const finalhandler = require('finalhandler');
const serveStatic = require('serve-static');

// This script is run as "node server.js path/to/distribution"
// Take path to files to serve from command line arg 2
const distribution_path = process.argv[2];

const brotliMiddleware = compression({
  filter: (req, res) => {
    if (req.headers['accept-encoding'] && req.headers['accept-encoding'].includes('br')) {
      return compression.filter(req, res);
    }
    return false;
  },
  brotli: { quality: 11 } // Set the compression level (1-11)
});

const serve = serveStatic(
  distribution_path,
  {
    index: ['index.html'],
    setHeaders: (res, path) => {
      console.log(path);
      if (path.includes('.br')) {
        res.setHeader('Content-Encoding', 'br');
      }
      if (path.includes('.wasm')) {
        res.setHeader('Content-Type', 'application/wasm');
      }
    },
  }
);

const options = {
    key: fs.readFileSync('private.key'),
    cert: fs.readFileSync('certificate.crt')
};

const server = https.createServer(options, (req, res) => {
  const done = finalhandler(req, res);
  brotliMiddleware(req, res, () => {
    serve(req, res, done);
  });
});

const port = process.env.PORT || 8080;
server.listen(port, () => {
  console.log(`Server is running at https://localhost:${port}`);
});
