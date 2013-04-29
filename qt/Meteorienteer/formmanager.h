#ifndef FORMMANAGER_H
#define FORMMANAGER_H

#include <QObject>
#include "taivaanvahtiform.h"
#include "taivaanvahti.h"

class FormManager : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QString type READ type)
    Q_PROPERTY(QString label READ label NOTIFY labelChanged)
    Q_PROPERTY(QString prefilled READ prefilled NOTIFY prefilledChanged)

public:
    // Form submission data. Field, value
    typedef QPair<TaivaanvahtiField*, QString> FieldValue;

    explicit FormManager(QObject *parent = 0);
    Q_INVOKABLE void getForm();
    Q_INVOKABLE int fieldCount();
    Q_INVOKABLE bool previous();
    Q_INVOKABLE bool next();
    Q_INVOKABLE QString type();
    Q_INVOKABLE QString label();
    Q_INVOKABLE QString prefilled();
    Q_INVOKABLE QString setValue(QString value);
    Q_INVOKABLE QString setValueIndex(int index);
    Q_INVOKABLE QStringList values();
    Q_INVOKABLE void submit();
    Q_INVOKABLE void reset();
    Q_INVOKABLE void setCompass(qreal azimuth);
    Q_INVOKABLE void setCoordinate(double lat, double lon);

signals:
    void formReceived();
    void submitted(bool success);
    void labelChanged();
    void prefilledChanged();

public slots:
    void receiveForm(TaivaanvahtiForm* form);
    void onSubmitted(bool success, int observationId, QString observationModificationKey);

private:
    void filter();
    QString validate(QString id, QString value);
    void saveSetting(QString id, QString value);
    QString loadSetting(QString id);
    void prefill();
    QString localizedLabel();
    QStringList localizedValues();

    Taivaanvahti* m_tv;
    TaivaanvahtiForm* m_form;
    // field, value, value index
    QList<QPair<FieldValue, int> > m_result;
    int m_currentField;
    TaivaanvahtiField* m_currentTaivaanvahtiField;
    QString m_currentId;
    QString m_currentValue;
    QStringList m_filterList;
    QString m_compassPoint;
    QString m_coordinate;
};

#endif // FORMMANAGER_H
