#ifndef TAIVAANVAHTI_H
#define TAIVAANVAHTI_H

#include <QObject>
#include <QNetworkAccessManager>
#include <QDomElement>
#include <QVector>
#include <QVariantList>

class TaivaanvahtiField;

/**
 * The Taivaanvahti class is a client class for
 * Taivaanvahti observation reporting system.
 *
 * Usage:
 * Connect formReceived() to your handler.
 * call getForm() to get the form. Category 1 would be fireballs.
 * formReceived() is emitted. In error fields is empty.
 */
class Taivaanvahti : public QObject
{
    Q_OBJECT
public:
    explicit Taivaanvahti(QObject *parent = 0);
    Q_INVOKABLE void getForm(int category);

signals:
    Q_INVOKABLE void formReceived(QVariantList fields);

private slots:
    void replyFinished(QNetworkReply* reply);
private:
    void handleCategory(QDomElement categoryElem, QVariantList &fields);
    QNetworkAccessManager nam;
};

#endif // TAIVAANVAHTI_H
