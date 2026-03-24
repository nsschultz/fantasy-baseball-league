FROM dpage/pgadmin4:6.6
COPY servers.json /pgadmin4/
RUN mkdir -p /var/lib/pgadmin/storage/admin_admin.com
COPY pgpass /var/lib/pgadmin/storage/admin_admin.com/
USER root
RUN chown pgadmin:pgadmin /var/lib/pgadmin/storage/admin_admin.com/pgpass && \
    chmod 0600 /var/lib/pgadmin/storage/admin_admin.com/pgpass
USER pgadmin
ENTRYPOINT ["/entrypoint.sh"]
