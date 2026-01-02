# Script to translate the newly added keys

$basePath = "c:\Users\flori\source\repos\Finova\Finova Nuget\src\Finova.Core\Resources"
$frFile = "$basePath\ValidationMessages.fr.resx"
$nlFile = "$basePath\ValidationMessages.nl.resx"
$deFile = "$basePath\ValidationMessages.de.resx"

$translations = @{
    "BelgiumIbanMustBeDigits" = @{
        fr = "L'IBAN belge doit être composé de chiffres.";
        nl = "Belgisch IBAN moet uit cijfers bestaan.";
        de = "Belgische IBAN muss aus Ziffern bestehen."
    };
    "BulgariaIbanInvalidAccount" = @{
        fr = "Compte IBAN bulgare invalide.";
        nl = "Ongeldige Bulgaarse IBAN-rekening.";
        de = "Ungültiges bulgarisches IBAN-Konto."
    };
    "BulgariaIbanInvalidBranchCode" = @{
        fr = "Code agence IBAN bulgare invalide.";
        nl = "Ongeldige Bulgaarse IBAN-filiaalcode.";
        de = "Ungültiger bulgarischer IBAN-Filialcode."
    };
    "BulgariaIbanInvalidControlChar" = @{
        fr = "Caractère de contrôle IBAN bulgare invalide.";
        nl = "Ongeldig Bulgaars IBAN-controleteken.";
        de = "Ungültiges bulgarisches IBAN-Kontrollzeichen."
    };
    "BulgariaUicInvalidChecksum10" = @{
        fr = "Somme de contrôle UIC bulgare invalide (10 chiffres).";
        nl = "Ongeldige Bulgaarse UIC-controlesom (10 cijfers).";
        de = "Ungültige bulgarische UIC-Prüfsumme (10 Ziffern)."
    };
    "BulgariaUicInvalidChecksum11" = @{
        fr = "Somme de contrôle UIC bulgare invalide (11 chiffres).";
        nl = "Ongeldige Bulgaarse UIC-controlesom (11 cijfers).";
        de = "Ungültige bulgarische UIC-Prüfsumme (11 Ziffern)."
    };
    "BulgariaUicInvalidChecksum12" = @{
        fr = "Somme de contrôle UIC bulgare invalide (12 chiffres).";
        nl = "Ongeldige Bulgaarse UIC-controlesom (12 cijfers).";
        de = "Ungültige bulgarische UIC-Prüfsumme (12 Ziffern)."
    };
    "EstoniaIbanMustBeDigits" = @{
        fr = "L'IBAN estonien doit être composé de chiffres.";
        nl = "Ests IBAN moet uit cijfers bestaan.";
        de = "Estnische IBAN muss aus Ziffern bestehen."
    };
    "HungaryIbanMustBeDigits" = @{
        fr = "L'IBAN hongrois doit être composé de chiffres.";
        nl = "Hongaars IBAN moet uit cijfers bestaan.";
        de = "Ungarische IBAN muss aus Ziffern bestehen."
    };
    "IcelandIbanMustContainOnlyDigits" = @{
        fr = "L'IBAN islandais ne doit contenir que des chiffres.";
        nl = "IJslands IBAN mag alleen cijfers bevatten.";
        de = "Isländische IBAN darf nur Ziffern enthalten."
    };
    "InvalidAustriaVatFormat" = @{
        fr = "Format de TVA autrichien invalide.";
        nl = "Ongeldig Oostenrijks BTW-formaat.";
        de = "Ungültiges österreichisches USt-Format."
    };
    "InvalidBelgiumOgmVcsReference" = @{
        fr = "Référence OGM/VCS belge invalide.";
        nl = "Ongeldige Belgische OGM/VCS-referentie.";
        de = "Ungültige belgische OGM/VCS-Referenz."
    };
    "InvalidBelgiumVatFormat" = @{
        fr = "Format de TVA belge invalide.";
        nl = "Ongeldig Belgisch BTW-formaat.";
        de = "Ungültiges belgisches USt-Format."
    };
    "InvalidBosniaAndHerzegovinaVatChecksum" = @{
        fr = "Somme de contrôle de TVA de Bosnie-Herzégovine invalide.";
        nl = "Ongeldige Bosnische BTW-controlesom.";
        de = "Ungültige bosnische USt-Prüfsumme."
    };
    "InvalidBosniaAndHerzegovinaVatChecksumRemainderOne" = @{
        fr = "Somme de contrôle de TVA de Bosnie-Herzégovine invalide (reste 1).";
        nl = "Ongeldige Bosnische BTW-controlesom (rest 1).";
        de = "Ungültige bosnische USt-Prüfsumme (Rest 1)."
    };
    "InvalidBosniaAndHerzegovinaVatFormat" = @{
        fr = "Format de TVA de Bosnie-Herzégovine invalide.";
        nl = "Ongeldig Bosnisch BTW-formaat.";
        de = "Ungültiges bosnisches USt-Format."
    };
    "InvalidCin" = @{
        fr = "CIN invalide.";
        nl = "Ongeldige CIN.";
        de = "Ungültige CIN."
    };
    "InvalidCnpjRepeated" = @{
        fr = "Chiffres CNPJ répétés invalides.";
        nl = "Ongeldige herhaalde CNPJ-cijfers.";
        de = "Ungültige wiederholte CNPJ-Ziffern."
    };
    "InvalidCpfRepeated" = @{
        fr = "Chiffres CPF répétés invalides.";
        nl = "Ongeldige herhaalde CPF-cijfers.";
        de = "Ungültige wiederholte CPF-Ziffern."
    };
    "InvalidEstoniaBbanStructure" = @{
        fr = "Structure BBAN estonienne invalide.";
        nl = "Ongeldige Estse BBAN-structuur.";
        de = "Ungültige estnische BBAN-Struktur."
    };
    "InvalidEstoniaRegistrikoodChecksum" = @{
        fr = "Somme de contrôle Registrikood estonien invalide.";
        nl = "Ongeldige Estse Registrikood-controlesom.";
        de = "Ungültige estnische Registrikood-Prüfsumme."
    };
    "InvalidEstoniaRegistrikoodFormat" = @{
        fr = "Format Registrikood estonien invalide.";
        nl = "Ongeldig Ests Registrikood-formaat.";
        de = "Ungültiges estnisches Registrikood-Format."
    };
    "InvalidEstoniaVatChecksum" = @{
        fr = "Somme de contrôle de TVA estonien invalide.";
        nl = "Ongeldige Estse BTW-controlesom.";
        de = "Ungültige estnische USt-Prüfsumme."
    };
    "InvalidEstoniaVatFormat" = @{
        fr = "Format de TVA estonien invalide.";
        nl = "Ongeldig Ests BTW-formaat.";
        de = "Ungültiges estnisches USt-Format."
    };
    "InvalidHungaryAdoszamLength" = @{
        fr = "Longueur Adószám hongrois invalide.";
        nl = "Ongeldige Hongaarse Adószám-lengte.";
        de = "Ungültige ungarische Adószám-Länge."
    };
    "InvalidHungaryVatChecksum" = @{
        fr = "Somme de contrôle de TVA hongrois invalide.";
        nl = "Ongeldige Hongaarse BTW-controlesom.";
        de = "Ungültige ungarische USt-Prüfsumme."
    };
    "InvalidHungaryVatFormat" = @{
        fr = "Format de TVA hongrois invalide.";
        nl = "Ongeldig Hongaars BTW-formaat.";
        de = "Ungültiges ungarisches USt-Format."
    };
    "InvalidIcelandCountryCode" = @{
        fr = "Code pays islandais invalide.";
        nl = "Ongeldige IJslandse landcode.";
        de = "Ungültiger isländischer Ländercode."
    };
    "InvalidIcelandKennitala" = @{
        fr = "Kennitala islandais invalide.";
        nl = "Ongeldige IJslandse Kennitala.";
        de = "Ungültige isländische Kennitala."
    };
    "InvalidItalyCountryCode" = @{
        fr = "Code pays italien invalide.";
        nl = "Ongeldige Italiaanse landcode.";
        de = "Ungültiger italienischer Ländercode."
    };
    "InvalidItalyIuvCheckDigit" = @{
        fr = "Chiffre de contrôle IUV italien invalide.";
        nl = "Ongeldig Italiaans IUV-controlecijfer.";
        de = "Ungültige italienische IUV-Prüfziffer."
    };
    "InvalidItalyIuvLength" = @{
        fr = "Longueur IUV italien invalide.";
        nl = "Ongeldige Italiaanse IUV-lengte.";
        de = "Ungültige italienische IUV-Länge."
    };
    "InvalidItalyIuvReference" = @{
        fr = "Référence IUV italienne invalide.";
        nl = "Ongeldige Italiaanse IUV-referentie.";
        de = "Ungültige italienische IUV-Referenz."
    };
    "InvalidKennitalaChecksum" = @{
        fr = "Somme de contrôle Kennitala invalide.";
        nl = "Ongeldige Kennitala-controlesom.";
        de = "Ungültige Kennitala-Prüfsumme."
    };
    "InvalidKennitalaDate" = @{
        fr = "Date Kennitala invalide.";
        nl = "Ongeldige Kennitala-datum.";
        de = "Ungültiges Kennitala-Datum."
    };
    "InvalidKennitalaFormat" = @{
        fr = "Format Kennitala invalide.";
        nl = "Ongeldig Kennitala-formaat.";
        de = "Ungültiges Kennitala-Format."
    };
    "InvalidKosovoFiscalNumberLength" = @{
        fr = "Longueur du numéro fiscal du Kosovo invalide.";
        nl = "Ongeldige lengte fiscaal nummer Kosovo.";
        de = "Ungültige Länge der kosovarischen Steuernummer."
    };
    "InvalidLatviaCountryCode" = @{
        fr = "Code pays letton invalide.";
        nl = "Ongeldige Letse landcode.";
        de = "Ungültiger lettischer Ländercode."
    };
    "InvalidLatviaPvnLength" = @{
        fr = "Longueur PVN letton invalide.";
        nl = "Ongeldige Letse PVN-lengte.";
        de = "Ungültige lettische PVN-Länge."
    };
    "InvalidLatviaVatChecksum" = @{
        fr = "Somme de contrôle de TVA letton invalide.";
        nl = "Ongeldige Letse BTW-controlesom.";
        de = "Ungültige lettische USt-Prüfsumme."
    };
    "InvalidLatviaVatChecksumResultMinusOne" = @{
        fr = "Somme de contrôle de TVA letton invalide (résultat -1).";
        nl = "Ongeldige Letse BTW-controlesom (resultaat -1).";
        de = "Ungültige lettische USt-Prüfsumme (Ergebnis -1)."
    };
    "InvalidLatviaVatFormat" = @{
        fr = "Format de TVA letton invalide.";
        nl = "Ongeldig Lets BTW-formaat.";
        de = "Ungültiges lettisches USt-Format."
    };
    "InvalidLiechtensteinCountryCode" = @{
        fr = "Code pays du Liechtenstein invalide.";
        nl = "Ongeldige Liechtensteinse landcode.";
        de = "Ungültiger liechtensteinischer Ländercode."
    };
    "InvalidLiechtensteinPeidChecksumCheckDigit10" = @{
        fr = "Somme de contrôle PEID du Liechtenstein invalide (chiffre 10).";
        nl = "Ongeldige Liechtensteinse PEID-controlesom (cijfer 10).";
        de = "Ungültige liechtensteinische PEID-Prüfsumme (Ziffer 10)."
    };
    "InvalidLiechtensteinPeidLength" = @{
        fr = "Longueur PEID du Liechtenstein invalide.";
        nl = "Ongeldige Liechtensteinse PEID-lengte.";
        de = "Ungültige liechtensteinische PEID-Länge."
    };
    "ItalyAbiCabMustBeDigits" = @{
        fr = "ABI/CAB italien doivent être des chiffres.";
        nl = "Italiaanse ABI/CAB moeten cijfers zijn.";
        de = "Italienische ABI/CAB müssen Ziffern sein."
    };
    "ItalyAccountNumberMustBeAlphanumeric" = @{
        fr = "Le numéro de compte italien doit être alphanumérique.";
        nl = "Italiaans rekeningnummer moet alfanumeriek zijn.";
        de = "Italienische Kontonummer muss alphanumerisch sein."
    };
    "ItalyCinMustBeLetter" = @{
        fr = "Le CIN italien doit être une lettre.";
        nl = "Italiaanse CIN moet een letter zijn.";
        de = "Italienische CIN muss ein Buchstabe sein."
    };
    "KennitalaCannotBeEmpty" = @{
        fr = "Kennitala ne peut pas être vide.";
        nl = "Kennitala mag niet leeg zijn.";
        de = "Kennitala darf nicht leer sein."
    };
    "LatviaIbanMustBeAlphanumeric" = @{
        fr = "L'IBAN letton doit être alphanumérique.";
        nl = "Lets IBAN moet alfanumeriek zijn.";
        de = "Lettische IBAN muss alphanumerisch sein."
    };
    "RegistrikoodCannotBeEmpty" = @{
        fr = "Registrikood ne peut pas être vide.";
        nl = "Registrikood mag niet leeg zijn.";
        de = "Registrikood darf nicht leer sein."
    };
}

function Update-File {
    param(
        [string]$FilePath,
        [string]$LangCode
    )
    
    Write-Host "Updating $FilePath..."
    $content = Get-Content $FilePath -Raw -Encoding UTF8
    
    foreach ($key in $translations.Keys) {
        $trans = $translations[$key][$LangCode]
        
        # Regex to match the data block for this key
        # We look for <data name="KEY" ...> ... <value>OLD_VALUE</value> ... </data>
        # We'll use a simpler replace for the value part if we can find the key
        
        if ($content -match "name=""$key""") {
            # We found the key. Now we need to replace the value.
            # Since the value might be multi-line or have spaces, we'll try to match the whole block
            
            # Construct a regex that matches the data element for this key
            # Note: The previous script added them at the end, likely in a standard format
            # <data name="KEY" xml:space="preserve">\r\n    <value>OLD_VALUE</value>\r\n  </data>
            
            # We will use a regex replace that captures the key and replaces the value
            $pattern = "(<data name=""$key""[^>]*>\s*<value>)([^<]*)(</value>)"
            $content = [regex]::Replace($content, $pattern, "${1}$trans${3}")
        }
    }
    
    $Utf8BomEncoding = New-Object System.Text.UTF8Encoding $true
    [System.IO.File]::WriteAllText($FilePath, $content, $Utf8BomEncoding)
}

Update-File -FilePath $frFile -LangCode "fr"
Update-File -FilePath $nlFile -LangCode "nl"
Update-File -FilePath $deFile -LangCode "de"

Write-Host "Translations applied!"
