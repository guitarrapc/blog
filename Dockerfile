FROM golang

RUN go install github.com/x-motemen/blogsync@latest

ENTRYPOINT [ "blogsync" ]