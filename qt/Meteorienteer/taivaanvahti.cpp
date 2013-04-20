#include "taivaanvahti.h"
#include <QNetworkRequest>
#include <QNetworkReply>
#include <QDebug>
#include <QDomDocument>
#include <QFile>
#include "taivaanvahtifield.h"
#include "taivaanvahtiform.h"

Taivaanvahti::Taivaanvahti(QObject *parent) :
    QObject(parent)
{}

void Taivaanvahti::getForm(int category)
{
    QNetworkRequest request;
    request.setUrl(QUrl("https://www.taivaanvahti.fi/api"));
    request.setHeader(QNetworkRequest::ContentTypeHeader, "text/xml");
    QString requestString = QString("<Request><Action>FormTemplateRequest</Action>"
                                    "<Category>%1</Category>"
                                    "</Request>").arg(category);

    QNetworkReply *reply = nam.post(request, requestString.toUtf8());
    connect(reply, SIGNAL(finished()), this, SLOT(getFormFinished()));
}

void Taivaanvahti::submitForm(QMap<TaivaanvahtiField *, QString> form, int category)
{
    QNetworkRequest request;
    request.setUrl(QUrl("https://www.taivaanvahti.fi/api"));
    request.setHeader(QNetworkRequest::ContentTypeHeader, "text/xml");
    QString outString;
#ifndef LAHETAOIKEASTI
    QDomDocument doc;
    QDomElement requestElement = doc.createElement("Request");
    doc.appendChild(requestElement);
    QDomElement actionElement = doc.createElement("Action");
    requestElement.appendChild(actionElement);
    QDomText actionText = doc.createTextNode("ObservationAddRequest");
    actionElement.appendChild(actionText);
    QDomElement observationElement = doc.createElement("observation");
    requestElement.appendChild(observationElement);
    foreach(TaivaanvahtiField *field, form.keys()) {
//        if(field->isMandatory())
            field->createFieldElement(observationElement, form.value(field), doc);
    }
    // Create category id
    QDomElement fieldElement = doc.createElement("field");
    observationElement.appendChild(fieldElement);

    QDomElement fieldIdElement = doc.createElement("field_id");
    fieldElement.appendChild(fieldIdElement);

    QDomText fieldIdText = doc.createTextNode("category_id");
    fieldIdElement.appendChild(fieldIdText);

    QDomElement fieldValueElement = doc.createElement("field_value");
    fieldElement.appendChild(fieldValueElement);
    QDomText fieldValueText = doc.createTextNode(QString::number(category));
    fieldValueElement.appendChild(fieldValueText);
    // end category id
    // add category
    QDomElement categoryElement = doc.createElement("category");
    observationElement.appendChild(categoryElement);
    TaivaanvahtiField::createFieldElement(categoryElement, "category_id", "tulipallo", doc);
    TaivaanvahtiField::createFieldElement(categoryElement, "specific_havaintokategoria", "Tulipallo", doc);
    // end category
    outString = doc.toString();
#else
    outString = readFile("testi.xml");
#endif
    qDebug() << Q_FUNC_INFO << "Out: " << outString;
    QNetworkReply *reply = nam.post(request, outString.toUtf8());
    connect(reply, SIGNAL(finished()), this, SLOT(submitFormFinished()));
}

QString Taivaanvahti::readFile(QString filename) {
  QFile file(filename);
  if (!file.open(QIODevice::ReadOnly | QIODevice::Text)) {
     return NULL;
  }

  QByteArray total;
  QByteArray line;
  while (!file.atEnd()) {
     line = file.read(1024);
     total.append(line);
  }

  return QString::fromUtf8(total);
}

void Taivaanvahti::getFormFinished()
{
    QNetworkReply *reply = qobject_cast<QNetworkReply*>(sender());
    QString replyString = QString::fromUtf8(reply->readAll());
    TaivaanvahtiForm* form = new TaivaanvahtiForm();
    QVector<TaivaanvahtiField*> fields;
    QDomDocument doc;
    if(doc.setContent(replyString)) {
        // qDebug() << Q_FUNC_INFO << doc.toString();
        QDomElement docElem = doc.documentElement();

        QDomElement categoryElem = docElem.firstChildElement();
        while(!categoryElem.isNull()) {
            if(categoryElem.tagName()=="observation" || categoryElem.tagName()=="category") {
                handleCategory(categoryElem, fields, form);
            }
            categoryElem = categoryElem.nextSiblingElement();
        }
    }
    form->setFields(fields);
    emit formReceived(form); // Ownership changes!
}

void Taivaanvahti::submitFormFinished()
{
    QNetworkReply *reply = qobject_cast<QNetworkReply*>(sender());
    QString replyString = QString::fromUtf8(reply->readAll());
    qDebug() << Q_FUNC_INFO << replyString;
    bool success = false;
    int id = 0;
    QString key;
    QDomDocument doc;
    if(doc.setContent(replyString)) {
        QDomElement docElem = doc.documentElement();
        QDomElement responseTypeElem = docElem.firstChildElement("response_type");
        success = responseTypeElem.text()=="Success";
        QDomElement observationIdElem = docElem.firstChildElement("observation_id");
        id = observationIdElem.text().toInt();
        QDomElement observationKeyElem = docElem.firstChildElement("observation_modification_key");
        key = observationKeyElem.text();
    }
    qDebug() << Q_FUNC_INFO << "success: " << success << " id: " << id << "key:" << key;
    emit formSubmitted(success, id, key);
}

void Taivaanvahti::handleCategory(QDomElement categoryElem, QVector<TaivaanvahtiField*> &fields, TaivaanvahtiForm* form)
{
    QDomElement e = categoryElem.firstChildElement();
    while(!e.isNull()) {
        if(e.tagName()=="field" || e.tagName()=="specific") {
            TaivaanvahtiField *field = new TaivaanvahtiField(form);
            field->parseFieldElement(e);
            fields.append(field);
        }
        e = e.nextSiblingElement();
    }
}

