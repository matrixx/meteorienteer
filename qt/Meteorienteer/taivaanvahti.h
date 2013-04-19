#ifndef TAIVAANVAHTI_H
#define TAIVAANVAHTI_H

#include <QObject>
#include <QNetworkAccessManager>
#include <QDomElement>
#include <QVector>

class TaivaanvahtiField;

class Taivaanvahti : public QObject
{
    Q_OBJECT
public:
    explicit Taivaanvahti(QObject *parent = 0);
    void getForm();

signals:
    void formReceived(QVector<TaivaanvahtiField*> &fields);

private slots:
    void replyFinished(QNetworkReply* reply);
private:
    void handleCategory(QDomElement categoryElem, QVector<TaivaanvahtiField*> &fields);
    QNetworkAccessManager nam;
};

#endif // TAIVAANVAHTI_H
