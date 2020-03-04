docker build -t wubba-lub-store .

docker tag wubba-lub-store registry.heroku.com/wubba-lub-store/web

docker push registry.heroku.com/wubba-lub-store/web

heroku container:release web -a wubba-lub-store