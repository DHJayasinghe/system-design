// Karma configuration file, see link for more information
// https://karma-runner.github.io/1.0/config/configuration-file.html

module.exports = function (config) ***REMOVED***
  config.set(***REMOVED***
    basePath: '',
    frameworks: ['jasmine', '@angular-devkit/build-angular'],
    plugins: [
      require('karma-jasmine'),
      require('karma-chrome-launcher'),
      require('karma-jasmine-html-reporter'),
      require('karma-coverage'),
      require('@angular-devkit/build-angular/plugins/karma')
    ],
    client: ***REMOVED***
      jasmine: ***REMOVED***
        // you can add configuration options for Jasmine here
        // the possible options are listed at https://jasmine.github.io/api/edge/Configuration.html
        // for example, you can disable the random execution with `random: false`
        // or set a specific seed with `seed: 4321`
      ***REMOVED***,
      clearContext: false // leave Jasmine Spec Runner output visible in browser
    ***REMOVED***,
    jasmineHtmlReporter: ***REMOVED***
      suppressAll: true // removes the duplicated traces
    ***REMOVED***,
    coverageReporter: ***REMOVED***
      dir: require('path').join(__dirname, './coverage/facebook-spa'),
      subdir: '.',
      reporters: [
    ***REMOVED*** type: 'html' ***REMOVED***,
    ***REMOVED*** type: 'text-summary' ***REMOVED***
      ]
    ***REMOVED***,
    reporters: ['progress', 'kjhtml'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ['Chrome'],
    singleRun: false,
    restartOnFileChange: true
  ***REMOVED***);
***REMOVED***;
