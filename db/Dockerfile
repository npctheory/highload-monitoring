FROM postgres:16-alpine
USER root

COPY initdb /docker-entrypoint-initdb.d/
COPY docker-healthcheck /docker-healthcheck/

USER postgres
ENTRYPOINT ["docker-entrypoint.sh"]
EXPOSE 5432
CMD ["postgres"]
