var fs = require('fs');
var path = require('path');

if( process.argv.length < 3 ){
    console.log('No metric log path specified. Use node analyser path-to-metrics-logs');
    process.exit(-1);
}
else {
    var metricsPath = process.argv[2];

    fs.access( metricsPath, fs.F_OK | fs.R_OK, function(err){
        if( err ){
            metricsPath = path.join( __dirname, metricsPath );
            fs.access( metricsPath, fs.F_OK | fs.R_OK, function(err){
                if( err ){
                    console.log( 'Failed to read directory ', metricsPath );
                    process.exit(-1);
                }
                else {
                    ProcessDir( metricsPath );
                }
            });
        }
        else {
            ProcessDir( metricsPath );
        }
    });
}

function ProcessDir( metricsDirectory ){
    fs.readdir( metricsDirectory, function(err, files){
        if( err ){
            console.log( 'Failed to open directory ', metricsDirectory );
            process.exit( -1 );
        }
        else {
            var totalTime = 0;
            var totalKills = 0;

            for( var i = 0; i < files.length; ++i ){
                if( files[i].indexOf('.json') >= 0 ){
                    console.log(files[i]);
                    var filePath = path.join( metricsDirectory, files[i] );
                    var thisFile = require( filePath );

                    if( thisFile && thisFile.StartTime && thisFile.EndTime && thisFile.EnemiesDestroyed ){
                        totalTime += thisFile.EndTime - thisFile.StartTime;
                        totalKills += thisFile.EnemiesDestroyed;
                    }
                }
            }

            var avgTime = totalTime / files.length;
            var avgKills = totalKills / files.length;

            console.log( 'Total Sessions: ', files.length );
            console.log( 'Total Time: ', totalTime );
            console.log( 'Total Kills: ' + totalKills );
            console.log( 'Average Time: ', avgTime );
            console.log( 'Average Kills: ', avgKills );
        }
    });
}