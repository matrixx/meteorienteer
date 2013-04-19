#include "taivaanvahti.h"
#include <QNetworkRequest>
#include <QNetworkReply>
#include <QDebug>

Taivaanvahti::Taivaanvahti(QObject *parent) :
    QObject(parent)
{
    connect(&nam, SIGNAL(finished(QNetworkReply*)),
            this, SLOT(replyFinished(QNetworkReply*)));
}

void Taivaanvahti::getForm()
{
    QNetworkRequest request;
    request.setUrl(QUrl("https://www.taivaanvahti.fi/api"));
    request.setHeader(QNetworkRequest::ContentTypeHeader, "text/xml");
    QByteArray data;
    data = "<Request><Action>FormTemplateRequest</Action>"
            "<Category>1</Category>"
            "</Request>";

    nam.post(request, data);
}

void Taivaanvahti::replyFinished(QNetworkReply *reply)
{
    qDebug() << Q_FUNC_INFO << QString::fromUtf8(reply->readAll());
}
