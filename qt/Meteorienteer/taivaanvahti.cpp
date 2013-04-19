#include "taivaanvahti.h"
#include <QNetworkRequest>
#include <QNetworkReply>
#include <QDebug>
#include <QDomDocument>

#include "taivaanvahtifield.h"

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
    QString replyString = QString::fromUtf8(reply->readAll());
    QVector<TaivaanvahtiField*> fields;
    QDomDocument doc;
    if(doc.setContent(replyString)) {
        // qDebug() << Q_FUNC_INFO << doc.toString();
        QDomElement docElem = doc.documentElement();
        QDomElement categoryElem = docElem.firstChildElement("observation");
        if(!categoryElem.isNull()) {
            handleCategory(categoryElem, fields);
        } else {
            qDebug() << Q_FUNC_INFO << "Error: invalid XML received.";
        }
        categoryElem = docElem.firstChildElement("category");
        if(!categoryElem.isNull()) {
            handleCategory(categoryElem, fields);
        } else {
            qDebug() << Q_FUNC_INFO << "Error: invalid XML received.";
        }
    }
    emit formReceived(fields);
}

void Taivaanvahti::handleCategory(QDomElement categoryElem, QVector<TaivaanvahtiField *> &fields)
{
    QDomElement e = categoryElem.firstChildElement();
    while(!e.isNull()) {
        if(e.tagName()=="field" || e.tagName()=="specific") {
            TaivaanvahtiField *field = new TaivaanvahtiField(this);
            field->parseFieldElement(e);
            fields.append(field);
        }
        e = e.nextSiblingElement();
    }
}

