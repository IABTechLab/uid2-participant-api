# docker commands

Build the image:
`docker build -t uid2-participant-api-image .`

Run the container:
`docker run -d -p 6001:8080 --name uid2-participant-api-image-container uid2-participant-api-image`

Go here to test:
http://localhost:6001/swagger/index.html