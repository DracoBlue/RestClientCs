#!/bin/bash
apt-get update
apt-get install mono-devel mono-gmcs nunit-console --fix-missing
apt-get install nginx php5-fpm --fix-missing
cd `dirname $0`
cp nginx.conf /etc/nginx/nginx.conf
echo "Restart NGINX"
/etc/init.d/nginx restart
echo "Restart PHP-FPM"
/etc/init.d/php5-fpm restart
