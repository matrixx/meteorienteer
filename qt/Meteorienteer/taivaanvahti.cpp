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

void Taivaanvahti::getForm(int category)
{
    QNetworkRequest request;
    request.setUrl(QUrl("https://www.taivaanvahti.fi/api"));
    request.setHeader(QNetworkRequest::ContentTypeHeader, "text/xml");
    QString requestString = QString("<Request><Action>FormTemplateRequest</Action>"
                                    "<Category>%1</Category>"
                                    "</Request>").arg(category);

    nam.post(request, requestString.toUtf8());
}

void Taivaanvahti::replyFinished(QNetworkReply *reply)
{
    QString replyString = QString::fromUtf8(reply->readAll());
    QVector<TaivaanvahtiField*> fields;
    QDomDocument doc;
    if(doc.setContent(replyString)) {
        // qDebug() << Q_FUNC_INFO << doc.toString();
        QDomElement docElem = doc.documentElement();

        QDomElement categoryElem = docElem.firstChildElement();
        while(!categoryElem.isNull()) {
            if(categoryElem.tagName()=="observation" || categoryElem.tagName()=="category") {
                handleCategory(categoryElem, fields);
            }
            categoryElem = categoryElem.nextSiblingElement();
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

