FROM node:19.1.0
RUN npm install --ignore-scripts -g mountebank@2.9.1 && \
    useradd -u 5000 mb && mkdir /app && chown -R mb:mb /app
WORKDIR /app
ENTRYPOINT ["mb"]
CMD ["start", "--configfile", "/app/imposters.ejs", "--allowInjection"]
USER mb
COPY *.ejs .
