docker-compose -f docker-compose.yml -p envsambaalpine stop
docker-compose  -f docker-compose.yml -p envsambaalpine down
TIMEOUT /T 1  /NOBREAK

docker-compose -f docker-compose.yml -p envsambaalpine up -d --build --remove-orphans