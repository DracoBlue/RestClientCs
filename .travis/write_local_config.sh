#!/bin/bash

cd `dirname $0`
cd ../
mkdir -p RestClientCs/bin/Debug/

echo '<?xml version="1.0" encoding="utf-8"?>' > RestClientCs/bin/Debug/RestClientCs.dll.config
echo '<configuration><appSettings><add key="TestServerUrl" value="http://localhost:8080'`pwd`'/.travis/echo.php" /></appSettings></configuration>' > RestClientCs/bin/Debug/RestClientCs.dll.config

