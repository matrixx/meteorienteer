#ifndef TAIVAANVAHTI_H
#define TAIVAANVAHTI_H

#include <QObject>
#include <QNetworkAccessManager>

class Taivaanvahti : public QObject
{
    Q_OBJECT
public:
    explicit Taivaanvahti(QObject *parent = 0);
    void getForm();

signals:
    
public slots:
private slots:
    void replyFinished(QNetworkReply* reply);
private:
    QNetworkAccessManager nam;
};

#endif // TAIVAANVAHTI_H
