var Metalsmith = require('metalsmith');
var branch = require('metalsmith-branch');
var sass = require('metalsmith-sass');
var fs = require('fs');

var metalsmith = Metalsmith(__dirname);
  metalsmith
  .source('contents')
  .destination('build')

  .use(branch('*.scss')
    .use(sass())
  )

  .build(function (err) {
    if (err) throw err;
	fs.copyFile('build/style.css', 'src/Fluxera.HttpStatusCodes/wwwroot/css/style.css', (err) => {
		if (err) throw err;
		console.log('style.css was copied to wwwroot.');
	});
	fs.copyFile('build/open-sans-regular.woff', 'src/Fluxera.HttpStatusCodes/wwwroot/css/open-sans-regular.woff', (err) => {
		if (err) throw err;
		console.log('open-sans-regular.woff was copied to wwwroot.');
	});
	fs.copyFile('build/source-code-pro-700.woff', 'src/Fluxera.HttpStatusCodes/wwwroot/css/source-code-pro-700.woff', (err) => {
		if (err) throw err;
		console.log('source-code-pro-700.woff was copied to wwwroot.');
	});
	fs.copyFile('build/favicon.ico', 'src/Fluxera.HttpStatusCodes/wwwroot/favicon.ico', (err) => {
		if (err) throw err;
		console.log('favicon.ico was copied to wwwroot.');
	});
	fs.copyFile('build/robots.txt', 'src/Fluxera.HttpStatusCodes/wwwroot/robots.txt', (err) => {
		if (err) throw err;
		console.log('robots.txt was copied to wwwroot.');
	});
    console.log('Build successful!');
  });
