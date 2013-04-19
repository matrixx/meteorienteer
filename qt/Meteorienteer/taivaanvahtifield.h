#ifndef TAIVAANVAHTIFIELD_H
#define TAIVAANVAHTIFIELD_H

#include <QObject>
#include <QDomElement>
#include <QUrl>
#include <QMap>

typedef QMap<QString, QString> ValueList;

class TaivaanvahtiField : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QString id READ id)
    Q_PROPERTY(QString label READ label)
    Q_PROPERTY(QString info READ info)
    Q_PROPERTY(QUrl infoUrl READ infoUrl)
    Q_PROPERTY(bool isMandatory READ isMandatory)
    Q_PROPERTY(FieldType type READ type)
    Q_PROPERTY(ValueList values READ values);
public:
    enum FieldType {
        TYPE_NOT_SET=0,
        TYPE_TEXT,
        TYPE_CHECKBOX,
        TYPE_SELECTION,
        TYPE_DATE,
        TYPE_COORDINATE,
        TYPE_TIME
    };
    explicit TaivaanvahtiField(QObject *parent = 0);
    void parseFieldElement(QDomElement elem);
    Q_INVOKABLE QString id() {return fieldId;};
    Q_INVOKABLE QString label() {return fieldLabel;};
    Q_INVOKABLE QString info() {return fieldInfo;};
    Q_INVOKABLE QUrl infoUrl() {return fieldInfoUrl;};
    Q_INVOKABLE bool isMandatory() {return mandatory;};
    Q_INVOKABLE FieldType type() {return fieldType;};
    Q_INVOKABLE ValueList values() {return fieldValues;};
private:
    QString fieldId, fieldLabel, fieldInfo;
    QUrl fieldInfoUrl;
    bool mandatory;
    FieldType fieldType;
    QMap<QString, QString> fieldValues;
};

#endif // TAIVAANVAHTIFIELD_H