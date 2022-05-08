var Metalsmith = require('metalsmith');
var branch = require('metalsmith-branch');
var sass = require('metalsmith-sass');

var metalsmith = Metalsmith(__dirname);
  metalsmith
  .source('contents')
  .destination('build')

  .use(branch('*.scss')
    .use(sass())
  )

  .build(function (err) {
    if (err) throw err;
    console.log('Build successful!');
  });
