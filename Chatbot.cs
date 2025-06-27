using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;

namespace PartProg3
{
    //To view the second and third definition , you need to type out the word ""more""
    // Random Responses 
    // - What is phishing? - Type in the same prompt --> Different responses 
    // Memory Recall. 
    public class Chatbot
    {
        private string rememberedInterest = string.Empty;
        private string lastTopic = string.Empty;
        private int repeatCount = 0;



        // ✅ Only one declaration like this
        public string GetMoreInfo(string topic, int level)
        {
            // Defensive programming: avoid null or empty topic input
            if (string.IsNullOrWhiteSpace(topic))
            {
                return "Invalid topic.";
            }

            // Example implementation
            return $"More info on topic '{topic}' at level {level}.";
        }



        public Dictionary<string, List<(string Paragraph, List<string> Tips, ConsoleColor Color)>> topicResponses = new Dictionary<string, List<(string, List<string>, ConsoleColor)>>
        {
            ["cybersecurity"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🛡️ Cybersecurity Overview:\nCybersecurity refers to protecting systems, networks, and data from digital attacks or unauthorized access.",
            new List<string>
            {
                "🧱 Use firewalls to monitor incoming and outgoing traffic.",
                "🔄 Keep software updated to patch vulnerabilities.",
                "🔐 Encrypt sensitive data.",
                "👥 Train users on safe practices.",
                "🕵️‍♂️ Audit systems for unusual activity.",
                "🧪 Use penetration testing to find security gaps."
            }, ConsoleColor.Green),
            ("🛡️ Cybersecurity in Practice:\nGood cybersecurity means staying aware of threats and protecting against vulnerabilities in hardware and software.",
            new List<string>
            {
                "📶 Secure your wireless networks.",
                "🔍 Monitor login attempts and unusual user behavior.",
                "📚 Stay informed on the latest cybersecurity threats.",
                "⚠️ Use access controls and permissions wisely.",
                "🗃 Back up data in case of breaches.",
                "🔐 Encrypt sensitive communications."
            }, ConsoleColor.Green),
            ("🛡️ Cybersecurity Strategy:\nEstablishing a robust cybersecurity strategy involves not only tools, but policy and awareness training.",
            new List<string>
            {
                "🧠 Educate staff on phishing and scams.",
                "🔐 Enforce strong password policies.",
                "🧩 Use multi-layered security defenses.",
                "🚨 Respond quickly to suspected breaches.",
                "📝 Develop and maintain a cybersecurity policy.",
                "📋 Perform regular risk assessments."
            }, ConsoleColor.Green)
        },

            ["phishing"] = new List<(string, List<string>, ConsoleColor)>
{
    (
        "🎣 Phishing Warning:\nPhishing tricks users into giving up personal information via fake emails, websites, or messages that appear legitimate.",
        new List<string>
        {
            "📧 Always check the sender's email address for authenticity.",
            "🚫 Do not click on suspicious links or attachments.",
            "🔒 Ensure the website uses HTTPS before entering credentials.",
            "📎 Avoid opening unexpected or unknown attachments.",
            "📢 Be wary of messages that create a sense of urgency.",
            "🔍 Hover over links to preview the actual destination."
        },
        ConsoleColor.DarkCyan
    ),
    (
        "🎣 Types of Phishing:\nPhishing can take many forms including spear phishing (targeted), vishing (voice phishing), and smishing (SMS phishing).",
        new List<string>
        {
            "🎯 Spear phishing targets specific individuals using personal info.",
            "📱 Smishing uses SMS texts to trick users into clicking links.",
            "📞 Vishing is conducted over phone calls by pretending to be legitimate.",
            "🌐 Clone phishing involves duplicating real emails with malicious intent.",
            "⚠️ Avoid clicking unknown or suspicious links in any form.",
            "🧠 Recognize red flags in unsolicited communication."
        },
        ConsoleColor.DarkCyan
    ),
    (
        "🎣 Avoiding Phishing Traps:\nProtect yourself from phishing by staying vigilant and questioning unexpected requests for sensitive info.",
        new List<string>
        {
            "📬 Never respond to unknown or suspicious senders.",
            "🛑 Avoid providing sensitive information online unless necessary.",
            "🔐 Implement two-factor authentication (2FA).",
            "📚 Regularly educate yourself on new phishing tactics.",
            "📈 Report any suspicious communication to your IT department.",
            "💬 When in doubt, confirm requests through a separate channel."
        },
        ConsoleColor.DarkCyan
    )
},


            ["ransomware"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("💰 Ransomware Threat:\nRansomware encrypts your data and demands payment to restore access.",
            new List<string>
            {
                "💾 Backup data regularly.",
                "🛡️ Install trusted antivirus software.",
                "🚪 Disconnect infected devices immediately.",
                "📉 Report incidents to authorities.",
                "📬 Be cautious of downloads.",
                "🔐 Don’t pay the ransom – no guarantee!"
            }, ConsoleColor.DarkMagenta),
            ("💰 How Ransomware Spreads:\nRansomware can come through phishing emails, malicious websites, or infected software.",
            new List<string>
            {
                "📧 Don’t open suspicious email attachments.",
                "📁 Avoid downloading from untrusted sources.",
                "🧼 Keep systems updated and patched.",
                "🧠 Educate staff on how to recognize ransomware traps.",
                "🛑 Use app whitelisting to prevent unknown applications.",
                "🔍 Monitor systems for encryption activity."
            }, ConsoleColor.DarkMagenta),
            ("💰 Recovering from Ransomware:\nPreparation and fast action are critical when facing a ransomware attack.",
            new List<string>
            {
                "📁 Maintain secure and tested backups.",
                "⚠️ Isolate affected systems quickly.",
                "📞 Contact incident response teams.",
                "📋 Document and investigate the breach.",
                "🔐 Restore from backups, not from ransom.",
                "📢 Report the incident to authorities and stakeholders."
            }, ConsoleColor.DarkMagenta)
        },

            ["malware"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🐛 Malware Explained:\nMalware is malicious software designed to harm or exploit any programmable device or network.",
            new List<string>
            {
                "🔍 Use antivirus software and keep it updated.",
                "🚫 Avoid downloading files from untrusted sources.",
                "📧 Be cautious with email attachments.",
                "🛡️ Regularly scan your system for malware.",
                "🔄 Keep your OS and apps updated.",
                "👀 Monitor for unusual system behavior."
            }, ConsoleColor.Red),
            ("🐛 Types of Malware:\nMalware comes in many forms including viruses, worms, Trojans, spyware, and adware.",
            new List<string>
            {
                "🦠 Viruses attach to files and spread.",
                "🪱 Worms spread across networks automatically.",
                "🐴 Trojans disguise as legitimate software.",
                "👁️ Spyware secretly collects user info.",
                "📢 Adware displays unwanted ads.",
                "🔐 Use comprehensive security tools to detect all types."
            }, ConsoleColor.Red),
            ("🐛 Preventing Malware Infection:\nPrevention is key by practicing safe computing habits.",
            new List<string>
            {
                "🛡️ Enable firewalls and security settings.",
                "🚫 Don't click on suspicious links or pop-ups.",
                "📥 Download apps only from trusted sources.",
                "🔐 Use strong, unique passwords.",
                "📝 Backup important data regularly.",
                "🧑‍💻 Stay informed about emerging malware threats."
            }, ConsoleColor.Red)
        },

            ["password security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🔑 Password Security:\nStrong passwords are essential for protecting your accounts and data.",
            new List<string>
            {
                "🔒 Use complex passwords with letters, numbers, and symbols.",
                "🧠 Avoid using common words or easily guessable info.",
                "🔄 Change passwords regularly.",
                "🛡️ Use a password manager to store passwords securely.",
                "🚫 Never reuse passwords across multiple sites.",
                "📢 Enable multi-factor authentication whenever possible."
            }, ConsoleColor.Yellow),
            ("🔑 Creating Strong Passwords:\nCreate passwords that are hard to guess but easy for you to remember.",
            new List<string>
            {
                "📏 Use passwords at least 12 characters long.",
                "🔀 Mix uppercase, lowercase, numbers, and symbols.",
                "🧩 Use passphrases—strings of random words.",
                "🔐 Avoid personal info like birthdays or names.",
                "🧠 Change passwords after suspected compromise.",
                "🛡️ Regularly audit your passwords for strength."
            }, ConsoleColor.Yellow),
            ("🔑 Password Management:\nUse tools and best practices to handle your passwords safely.",
            new List<string>
            {
                "🗃️ Use trusted password managers.",
                "🔄 Update passwords especially for critical accounts.",
                "🚫 Avoid writing passwords down physically.",
                "🔐 Enable two-factor authentication (2FA).",
                "🛑 Never share your passwords.",
                "📢 Monitor your accounts for unauthorized access."
            }, ConsoleColor.Yellow)
        },

            ["social engineering"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🧠 Social Engineering:\nThis is the art of manipulating people into giving up confidential information.",
            new List<string>
            {
                "👥 Be cautious about unsolicited requests for info.",
                "🛑 Verify identities before sharing info.",
                "📞 Avoid sharing sensitive info on calls or emails.",
                "📚 Educate employees about common tactics.",
                "🔍 Question unusual requests or urgencies.",
                "📝 Report suspicious behavior immediately."
            }, ConsoleColor.Cyan),
            ("🧠 Common Social Engineering Attacks:\nPhishing, pretexting, baiting, and tailgating are frequent tactics.",
            new List<string>
            {
                "🎣 Phishing involves fraudulent emails or websites.",
                "🗣️ Pretexting is creating a fake scenario to get info.",
                "🎁 Baiting offers something to entice victims.",
                "🚪 Tailgating means following someone into secure areas.",
                "⚠️ Stay vigilant and skeptical of unsolicited contacts.",
                "🔒 Always confirm requests through trusted channels."
            }, ConsoleColor.Cyan),
            ("🧠 Preventing Social Engineering:\nDefense is awareness and strict security policies.",
            new List<string>
            {
                "🧑‍💻 Regular training sessions for all employees.",
                "🔐 Enforce access controls and verification.",
                "📢 Encourage reporting of suspicious attempts.",
                "📚 Keep updated on latest social engineering tactics.",
                "🛡️ Use technical controls like email filters.",
                "🕵️ Monitor and audit for unusual activities."
            }, ConsoleColor.Cyan)
        },

            ["firewalls"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🔥 Firewalls:\nFirewalls monitor and control incoming and outgoing network traffic based on security rules.",
            new List<string>
            {
                "🛡️ Use firewalls to block unauthorized access.",
                "⚙️ Configure firewall rules carefully.",
                "🔄 Keep firewall software and hardware updated.",
                "📊 Monitor firewall logs regularly.",
                "🚫 Restrict traffic to necessary ports and services.",
                "🔐 Use firewall alongside other security measures."
            }, ConsoleColor.DarkYellow),
            ("🔥 Types of Firewalls:\nThere are hardware, software, and cloud-based firewalls serving different purposes.",
            new List<string>
            {
                "🖥️ Hardware firewalls protect entire networks.",
                "💻 Software firewalls protect individual devices.",
                "☁️ Cloud firewalls secure cloud infrastructures.",
                "🔒 Use layered firewalls for better protection.",
                "📋 Regularly review firewall configurations.",
                "🔍 Test firewalls with penetration testing."
            }, ConsoleColor.DarkYellow),
            ("🔥 Firewall Best Practices:\nEffective firewall management enhances your security posture.",
            new List<string>
            {
                "🔄 Regularly update firewall firmware and rules.",
                "📢 Document firewall policies clearly.",
                "⚠️ Remove unnecessary rules to reduce risk.",
                "🔍 Conduct periodic firewall audits.",
                "📈 Monitor alerts and respond promptly.",
                "🛡️ Combine firewalls with intrusion detection systems."
            }, ConsoleColor.DarkYellow)
        },

            ["two-factor authentication"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🔐 Two-Factor Authentication (2FA):\n2FA adds an extra layer of security by requiring two forms of identification before granting access.",
            new List<string>
            {
                "📱 Use authenticator apps or SMS codes.",
                "🔒 Enable 2FA on all critical accounts.",
                "⚠️ Avoid using SMS 2FA if possible (less secure).",
                "🧠 Educate users on the benefits of 2FA.",
                "🔄 Regularly review 2FA settings.",
                "🛡️ Combine 2FA with strong passwords."
            }, ConsoleColor.Blue),
            ("🔐 Benefits of 2FA:\nIt drastically reduces the chances of unauthorized access.",
            new List<string>
            {
                "🚫 Prevents access even if passwords are compromised.",
                "🕵️ Alerts you to unauthorized login attempts.",
                "🛡️ Protects sensitive and financial data.",
                "📊 Increases compliance with security standards.",
                "🧩 Compatible with most online platforms.",
                "🔒 Simple to implement and use."
            }, ConsoleColor.Blue),
            ("🔐 Implementing 2FA:\nSetting up 2FA is straightforward and boosts your security significantly.",
            new List<string>
            {
                "📥 Download and install an authenticator app.",
                "🔑 Link your account to the app or device.",
                "🔄 Test login to ensure 2FA is working.",
                "🛡️ Backup recovery codes securely.",
                "📢 Inform all users about enabling 2FA.",
                "⚙️ Regularly update your 2FA methods."
            }, ConsoleColor.Blue)
        },

            ["data encryption"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🔐 Data Encryption:\nEncryption converts readable data into coded form to protect confidentiality.",
            new List<string>
            {
                "🔏 Use encryption for data at rest and in transit.",
                "🛡️ Choose strong encryption algorithms.",
                "🔑 Manage encryption keys securely.",
                "📚 Train staff on encryption best practices.",
                "🔒 Use HTTPS for secure web communication.",
                "📁 Encrypt backups and sensitive files."
            }, ConsoleColor.DarkGreen),
            ("🔐 Types of Encryption:\nSymmetric and asymmetric encryption serve different purposes.",
            new List<string>
            {
                "🔄 Symmetric uses one key for encryption and decryption.",
                "🔑 Asymmetric uses a public and private key pair.",
                "🔐 Use asymmetric for secure key exchange.",
                "🧩 Use symmetric for encrypting large data efficiently.",
                "🛡️ Combine both for best security practices.",
                "📊 Regularly update encryption protocols."
            }, ConsoleColor.DarkGreen),
            ("🔐 Benefits of Encryption:\nEncryption safeguards data against unauthorized access and breaches.",
            new List<string>
            {
                "🔒 Protects personal and sensitive info.",
                "🕵️ Maintains data integrity and authenticity.",
                "🛡️ Enables compliance with legal regulations.",
                "📡 Secures communications over insecure networks.",
                "📁 Reduces risk of data theft and loss.",
                "🧩 Supports secure storage and cloud services."
            }, ConsoleColor.DarkGreen)
        },

            ["backup and recovery"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("💾 Backup and Recovery:\nRegular backups ensure you can recover data after loss or attack.",
            new List<string>
            {
                "📅 Schedule regular backups.",
                "🗂 Store backups in secure, separate locations.",
                "🔄 Test backup restoration processes.",
                "🛡️ Encrypt backup data.",
                "🧠 Educate staff on backup importance.",
                "📊 Keep multiple backup versions."
            }, ConsoleColor.Gray),
            ("💾 Types of Backup:\nFull, incremental, and differential backups offer different recovery options.",
            new List<string>
            {
                "🔄 Full backups copy all data.",
                "⚡ Incremental backup copies changes since last backup.",
                "📝 Differential backup copies changes since last full backup.",
                "📦 Choose strategy based on recovery needs.",
                "📊 Balance storage space and backup time.",
                "🔐 Secure backup access."
            }, ConsoleColor.Gray),
            ("💾 Recovery Planning:\nPlan your recovery to minimize downtime and data loss.",
            new List<string>
            {
                "📋 Document recovery procedures.",
                "🧪 Perform regular disaster recovery drills.",
                "🔍 Identify critical data and systems.",
                "🛡️ Prioritize restoring essential services.",
                "📞 Assign roles for recovery efforts.",
                "📢 Communicate status during incidents."
            }, ConsoleColor.Gray)
        },

            ["safe browsing"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🌐 Safe Browsing:\nBe cautious about the websites you visit to avoid malware and scams.",
            new List<string>
            {
                "🔒 Use HTTPS websites.",
                "🧹 Clear cookies and cache regularly.",
                "🚫 Avoid clicking on suspicious ads.",
                "📢 Use browser security extensions.",
                "🔄 Keep your browser updated.",
                "🧠 Be wary of phishing sites."
            }, ConsoleColor.Magenta),
            ("🌐 Recognizing Unsafe Websites:\nCheck URLs carefully and watch for signs of fraud.",
            new List<string>
            {
                "⚠️ Avoid URLs with misspellings or extra characters.",
                "🔍 Look for the padlock icon in the address bar.",
                "📱 Use mobile browser security features.",
                "🛡️ Use anti-phishing tools and filters.",
                "🧩 Avoid downloading files from unknown sites.",
                "🕵️‍♂️ Check site reputation online."
            }, ConsoleColor.Magenta),
            ("🌐 Enhancing Browsing Security:\nTake steps to reduce risks while browsing the web.",
            new List<string>
            {
                "🛑 Don’t save passwords in browsers without encryption.",
                "🔐 Use a VPN on public Wi-Fi.",
                "📥 Avoid auto-downloads.",
                "🧠 Be skeptical of pop-ups requesting info.",
                "📢 Report suspicious websites.",
                "🔄 Regularly update browser plugins."
            }, ConsoleColor.Magenta)
        },

            ["password safety"] = new List<(string, List<string>, ConsoleColor)>
{
    ("🔑 Password Safety:\nStrong passwords are your first line of defense against cyber threats.",
    new List<string>
    {
        "🧠 Use long, complex passwords with a mix of letters, numbers, and symbols.",
        "🚫 Avoid using personal information like birthdays or names.",
        "🔁 Never reuse passwords across multiple sites.",
        "🔐 Enable two-factor authentication (2FA) wherever possible.",
        "🧾 Use a trusted password manager to store your credentials.",
        "📆 Change your passwords regularly, especially after a breach."
    }, ConsoleColor.Yellow),

    ("🔐 Creating Secure Passwords:\nWell-crafted passwords make it harder for hackers to access your accounts.",
    new List<string>
    {
        "✍️ Create passphrases using unrelated words (e.g., ‘BlueTiger$Pizza7!’).",
        "🎲 Use a password generator for randomness.",
        "👁️‍🗨️ Avoid dictionary words or common sequences (e.g., ‘123456’).",
        "🚷 Don’t write down passwords where others can find them.",
        "📲 Avoid syncing passwords across insecure devices.",
        "📧 Be cautious of emails asking you to reset passwords unexpectedly."
    }, ConsoleColor.Yellow),

    ("🔓 Managing Password Security:\nProper management of passwords keeps your digital identity safe.",
    new List<string>
    {
        "🛡️ Store passwords in encrypted vaults only.",
        "🧭 Audit your password list occasionally to remove unused accounts.",
        "🕵️‍♀️ Watch for data breach notifications.",
        "📢 Don’t share passwords over messaging apps.",
        "💡 Use different passwords for banking, email, and social media.",
        "🔍 Check for unusual login activity in your accounts."
    }, ConsoleColor.Yellow)
},


            ["wifi security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("📶 Wi-Fi Security:\nSecure your wireless network to prevent unauthorized access.",
            new List<string>
            {
                "🔒 Use WPA3 or WPA2 encryption.",
                "🚪 Change default router passwords.",
                "📡 Disable SSID broadcasting if possible.",
                "🛡️ Enable network firewalls.",
                "🧑‍💻 Regularly update router firmware.",
                "📊 Monitor connected devices."
            }, ConsoleColor.DarkBlue),
            ("📶 Public Wi-Fi Risks:\nPublic networks are often unsecured and risky.",
            new List<string>
            {
                "⚠️ Avoid accessing sensitive info on public Wi-Fi.",
                "🔐 Use a VPN when on public networks.",
                "🧠 Turn off sharing features on devices.",
                "📵 Disable automatic connection to open networks.",
                "🕵️‍♂️ Use HTTPS to encrypt data traffic.",
                "📞 Report suspicious network activity."
            }, ConsoleColor.DarkBlue),
            ("📶 Strengthening Wi-Fi Security:\nImplement best practices to protect your home or office network.",
            new List<string>
            {
                "🔄 Change Wi-Fi password regularly.",
                "🔧 Use MAC address filtering.",
                "🚫 Disable WPS (Wi-Fi Protected Setup).",
                "📶 Use guest networks for visitors.",
                "📢 Educate users about Wi-Fi risks.",
                "🛡️ Use enterprise-level security for businesses."
            }, ConsoleColor.DarkBlue)
        },

            ["software updates"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("⬆️ Software Updates:\nKeep software current to protect against known vulnerabilities.",
            new List<string>
            {
                "🔄 Enable automatic updates where possible.",
                "📅 Check for updates regularly.",
                "🛡️ Prioritize security patches.",
                "🧠 Educate users on update importance.",
                "📋 Maintain an update schedule.",
                "🔍 Test updates before full deployment."
            }, ConsoleColor.DarkRed),
            ("⬆️ Risks of Outdated Software:\nOld software can be exploited by attackers easily.",
            new List<string>
            {
                "⚠️ Increased risk of malware infections.",
                "🔓 Vulnerabilities remain unpatched.",
                "📉 Possible system instability.",
                "🛑 Compliance violations.",
                "🕵️‍♂️ Target for cyber attacks.",
                "🔧 Compatibility issues with new tech."
            }, ConsoleColor.DarkRed),
            ("⬆️ Best Practices for Updates:\nManage updates to maximize security and minimize disruptions.",
            new List<string>
            {
                "🗓 Schedule updates during off-peak hours.",
                "🔐 Back up data before updating.",
                "📢 Inform users about upcoming updates.",
                "🧪 Test updates on a subset of devices.",
                "🛠 Keep rollback plans ready.",
                "📊 Monitor for update-related issues."
            }, ConsoleColor.DarkRed)
        },

            ["incident response"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🚨 Incident Response:\nA plan to address and manage cybersecurity incidents effectively.",
            new List<string>
            {
                "📋 Develop a clear response plan.",
                "📞 Establish communication protocols.",
                "🧑‍💻 Assign roles and responsibilities.",
                "🔍 Detect and analyze incidents quickly.",
                "🛡️ Contain and mitigate damage.",
                "📢 Report incidents to stakeholders."
            }, ConsoleColor.DarkGray),
            ("🚨 Steps in Incident Response:\nPreparation, identification, containment, eradication, recovery, and lessons learned.",
            new List<string>
            {
                "⚙️ Prepare tools and team in advance.",
                "🔎 Identify the nature and scope.",
                "🚧 Contain to prevent spread.",
                "🧹 Remove threats and vulnerabilities.",
                "🔄 Recover systems and operations.",
                "📖 Document and learn for future."
            }, ConsoleColor.DarkGray),
            ("🚨 Improving Incident Response:\nRegularly update and practice your incident response plan.",
            new List<string>
            {
                "🧪 Conduct tabletop exercises.",
                "📚 Train staff regularly.",
                "📋 Update plans with new threats.",
                "🔍 Review response effectiveness.",
                "📢 Communicate transparently during incidents.",
                "🛡️ Invest in detection and monitoring tools."
            }, ConsoleColor.DarkGray)
        },

            ["physical security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🏢 Physical Security:\nProtecting hardware and physical access points from unauthorized entry.",
            new List<string>
            {
                "🚪 Use access badges and locks.",
                "🕵️ Monitor premises with cameras.",
                "📅 Control visitor access.",
                "🧰 Secure equipment in locked rooms.",
                "🛡️ Train staff on physical security.",
                "🔄 Regularly audit physical security measures."
            }, ConsoleColor.DarkGreen),
            ("🏢 Common Physical Threats:\nTheft, vandalism, and insider threats.",
            new List<string>
            {
                "🕵️‍♂️ Monitor suspicious behavior.",
                "🚨 Use alarms and alerts.",
                "📋 Limit access to sensitive areas.",
                "🔒 Secure backup media physically.",
                "🛠 Maintain hardware with tamper-proof features.",
                "🧠 Educate employees on reporting procedures."
            }, ConsoleColor.DarkGreen),
            ("🏢 Enhancing Physical Security:\nCombine technology and policies for best results.",
            new List<string>
            {
                "📹 Integrate surveillance with access control.",
                "🚪 Implement mantraps or security vestibules.",
                "🧑‍💻 Use multi-factor authentication for physical access.",
                "📢 Regularly update policies.",
                "🛡️ Conduct physical penetration tests.",
                "📝 Keep detailed access logs."
            }, ConsoleColor.DarkGreen)
        },

            ["insider threats"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("👤 Insider Threats:\nRisks posed by employees or contractors with legitimate access.",
            new List<string>
            {
                "🕵️ Monitor unusual user behavior.",
                "📋 Enforce least privilege access.",
                "🔐 Use strong access controls.",
                "🧠 Educate staff on security policies.",
                "📢 Encourage reporting of suspicious activity.",
                "📊 Conduct background checks."
            }, ConsoleColor.DarkCyan),
            ("👤 Types of Insider Threats:\nMalicious insiders, negligent insiders, and compromised insiders.",
            new List<string>
            {
                "🔪 Malicious insiders deliberately cause harm.",
                "😕 Negligent insiders cause breaches unintentionally.",
                "🔓 Compromised insiders have had credentials stolen.",
                "🧩 Use monitoring and alerting tools.",
                "📚 Provide continuous security awareness.",
                "🔒 Enforce data loss prevention (DLP) policies."
            }, ConsoleColor.DarkCyan),
            ("👤 Mitigating Insider Threats:\nCombining technical and human strategies.",
            new List<string>
            {
                "🔍 Regularly review access rights.",
                "🛡️ Deploy user behavior analytics.",
                "📢 Foster a positive security culture.",
                "🧑‍💼 Conduct exit interviews and revoke access promptly.",
                "📋 Implement strong policies and enforcement.",
                "🔄 Update mitigation strategies based on incidents."
            }, ConsoleColor.DarkCyan)
        },

            ["cloud security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("☁️ Cloud Security:\nProtecting data, applications, and services in cloud environments.",
            new List<string>
            {
                "🔐 Use encryption for data in cloud storage.",
                "🛡️ Implement strong identity and access management.",
                "📜 Understand your cloud provider’s security responsibilities.",
                "🔄 Regularly update cloud security policies.",
                "🧩 Monitor cloud environments continuously.",
                "📚 Train staff on cloud risks and best practices."
            }, ConsoleColor.DarkMagenta),
            ("☁️ Shared Responsibility Model:\nCloud security is a joint effort between provider and customer.",
            new List<string>
            {
                "☁️ Providers secure the infrastructure.",
                "🧑‍💼 Customers secure their data and access.",
                "🔍 Regular audits and compliance checks are essential.",
                "🔐 Use multi-factor authentication.",
                "📋 Document cloud usage policies clearly.",
                "🛡️ Keep software and APIs updated."
            }, ConsoleColor.DarkMagenta),
            ("☁️ Best Practices for Cloud Security:\nMaximize protection with layered defenses.",
            new List<string>
            {
                "🧩 Use network segmentation in the cloud.",
                "🔍 Enable logging and monitoring.",
                "📦 Apply least privilege access principles.",
                "🔄 Conduct regular vulnerability assessments.",
                "📢 Report cloud incidents promptly.",
                "🛡️ Stay updated on cloud security trends."
            }, ConsoleColor.DarkMagenta)
        },

            ["mobile security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("📱 Mobile Security:\nProtect mobile devices from threats such as malware, theft, and unauthorized access.",
            new List<string>
            {
                "🔒 Use strong screen locks.",
                "🛡️ Install security apps.",
                "📱 Update OS and apps regularly.",
                "🚫 Avoid downloading apps from unknown sources.",
                "📡 Use VPNs on public Wi-Fi.",
                "🧠 Be cautious with app permissions."
            }, ConsoleColor.Blue),
            ("📱 Mobile Threats:\nIncludes malicious apps, network attacks, and physical loss.",
            new List<string>
            {
                "📲 Verify app sources and reviews.",
                "⚠️ Beware of phishing attempts via SMS.",
                "🕵️‍♂️ Use device tracking features.",
                "🧩 Encrypt sensitive data on devices.",
                "🛡️ Regularly back up mobile data.",
                "📢 Educate users on mobile risks."
            }, ConsoleColor.Blue),
            ("📱 Best Practices for Mobile Security:\nCombine technology and awareness for protection.",
            new List<string>
            {
                "🔄 Keep devices updated.",
                "🔐 Use biometric authentication.",
                "🛑 Disable Bluetooth and Wi-Fi when not needed.",
                "📶 Use secure network connections.",
                "📋 Monitor app permissions regularly.",
                "🧑‍💻 Train users on mobile security."
            }, ConsoleColor.Blue)
        },

            ["email security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("📧 Email Security:\nEmail is a common attack vector; protect it rigorously.",
            new List<string>
            {
                "🔐 Use strong passwords and 2FA.",
                "📥 Beware of phishing and spoofing attempts.",
                "📎 Don’t open suspicious attachments.",
                "🧹 Regularly clean inbox.",
                "🛡️ Use email filtering solutions.",
                "📢 Educate users on email threats."
            }, ConsoleColor.Yellow),
            ("📧 Phishing Detection:\nRecognize signs of phishing emails.",
            new List<string>
            {
                "⚠️ Check sender addresses carefully.",
                "🔍 Look for spelling and grammar errors.",
                "🚫 Avoid clicking unknown links.",
                "🧩 Verify requests via separate channels.",
                "📞 Report phishing attempts.",
                "🛑 Don’t share sensitive info via email."
            }, ConsoleColor.Yellow),
            ("📧 Email Best Practices:\nMaintain hygiene and vigilance.",
            new List<string>
            {
                "🔄 Change passwords regularly.",
                "🔐 Use encrypted email services.",
                "📋 Archive important emails securely.",
                "🧑‍💻 Educate users regularly.",
                "⚠️ Monitor email access logs.",
                "🛡️ Use DMARC, SPF, and DKIM protocols."
            }, ConsoleColor.Yellow)
        },

            ["password security"] = new List<(string, List<string>, ConsoleColor)>
        {
            ("🔑 Password Security:\nStrong, unique passwords are the first defense line.",
            new List<string>
            {
                "🔐 Use complex passwords with letters, numbers, and symbols.",
                "🔄 Change passwords regularly.",
                "📋 Don’t reuse passwords across sites.",
                "🧠 Use password managers.",
                "🚫 Avoid easily guessable info.",
                "📢 Educate users on creating strong passwords."
            }, ConsoleColor.Cyan),
            ("🔑 Password Attack Types:\nBe aware of brute force, dictionary, and phishing attacks.",
            new List<string>
            {
                "🕵️‍♂️ Use account lockouts after failed attempts.",
                "⚠️ Beware of phishing scams.",
                "🔒 Use multi-factor authentication.",
                "🔄 Rotate passwords periodically.",
                "🛡️ Use salted and hashed storage.",
                "🧩 Educate users on safe password habits."
            }, ConsoleColor.Cyan),
            ("🔑 Password Management Best Practices:\nImplement policies to protect credentials effectively.",
            new List<string>
            {
                "📋 Enforce minimum length and complexity.",
                "🔐 Store passwords securely.",
                "🔄 Educate users on phishing risks.",
                "🧩 Use password vaults and managers.",
                "⚠️ Avoid writing passwords down.",
                "🛡️ Enable account recovery options securely."
            }, ConsoleColor.Cyan)
        },


        };

        Dictionary<string, int> topicUsageCount = new Dictionary<string, int>();


        // Updated Start method
        //If the user wants more information about a topic they simply type in "want more advice on topic , instead of topic use a word from the menu list e.g. like phishing
        //" and more advice is given 


        public string GenerateAnswer(string question, string name, string interest)
        {
            if (string.IsNullOrWhiteSpace(question))
                return "🤔 Please ask a valid cybersecurity question.";

            string lowered = question.ToLower();
            string? matchedTopic = topicResponses.Keys.FirstOrDefault(k => lowered.Contains(k));

            if (matchedTopic != null)
            {
                lastTopic = matchedTopic;

                if (!topicUsageCount.ContainsKey(matchedTopic))
                    topicUsageCount[matchedTopic] = 0;

                int count = topicUsageCount[matchedTopic];
                var response = topicResponses[matchedTopic][count];

                topicUsageCount[matchedTopic] = (count + 1) % topicResponses[matchedTopic].Count;

                string tipsText = "\n🧠 Here's some advice:\n";
                foreach (var tip in response.Tips)
                {
                    tipsText += $" - {tip}\n";
                }

                return $"Hi {name}, since you're interested in {interest}, here's what I can tell you about {matchedTopic}:\n\n" +
                       $"{response.Paragraph}{tipsText}";
            }

            return $"Hi {name}, I didn’t find a matching topic for “{question}.” Try asking about phishing, firewalls, or encryption.";
        }

        public void Start()
        {
            Console.WriteLine("👋 Welcome to the Cybersecurity Chatbot!");
            Console.WriteLine("🤖 Ask me anything about cybersecurity (20 topics available).");
            Console.WriteLine("Type 'exit' to quit.");
            Console.WriteLine("Type 'more' or 'want more advice' to get deeper tips after you ask a question.");

            bool canAskForMoreAdvice = false;
            string? lastTopic = null;
            int repeatCount = 0;
            string name = "User";
            string interest = "cybersecurity";

            while (true)
            {
                Console.Write("\nYou: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    canAskForMoreAdvice = false;
                    continue;
                }

                string trimmedInput = input.Trim().ToLower();

                if (trimmedInput == "exit")
                    break;

                if (trimmedInput == "more" || trimmedInput == "want more advice")
                {
                    if (canAskForMoreAdvice && lastTopic != null)
                    {
                        repeatCount++;
                        if (repeatCount < topicResponses[lastTopic].Count)
                        {
                            Console.WriteLine(FormatResponse(topicResponses[lastTopic][repeatCount]));

                            if (repeatCount < topicResponses[lastTopic].Count - 1)
                            {
                                Console.WriteLine("🤖 Tell me more or Want More advice?");
                                canAskForMoreAdvice = true;
                            }
                            else
                            {
                                Console.WriteLine("✅ You've received all the available advice for this topic.");
                                canAskForMoreAdvice = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("✅ You've received all the available advice for this topic.");
                            canAskForMoreAdvice = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("🤔 Please ask about a topic first before requesting more advice, or no more advice available.");
                        canAskForMoreAdvice = false;
                    }
                    continue;
                }

                string? matchedTopic = topicResponses.Keys.FirstOrDefault(k => trimmedInput.Contains(k));

                if (matchedTopic != null)
                {
                    if (lastTopic == matchedTopic)
                    {
                        repeatCount++;
                        if (repeatCount >= topicResponses[matchedTopic].Count)
                            repeatCount = 0;
                    }
                    else
                    {
                        lastTopic = matchedTopic;
                        repeatCount = 0;
                    }

                    string responseText = GenerateAnswer(input, name, interest);
                    Console.WriteLine("💬 " + responseText);

                    canAskForMoreAdvice = topicResponses[matchedTopic].Count > 1;
                    if (canAskForMoreAdvice)
                        Console.WriteLine("🤖 Tell me more or Want More advice?");
                }
                else
                {
                    Console.WriteLine("🤔 I'm not sure how to answer that. Try asking about 'cybersecurity', 'phishing', or 'ransomware'.");
                    canAskForMoreAdvice = false;
                }
            }

            Console.WriteLine("👋 Goodbye! Stay safe online!");

            string FormatResponse((string Paragraph, List<string> Tips, ConsoleColor Color) response)
            {
                Console.ForegroundColor = response.Color;

                string tipsText = "\n🧠 Here's some advice:\n";
                foreach (var tip in response.Tips)
                {
                    tipsText += $" - {tip}\n";
                }

                Console.ResetColor();

                return "Chatbot: " + response.Paragraph + tipsText;
            }

        }
    }
}
// Dev Details:
// Name: Kiara Israel
// Student Number: ST10277747
// Module: PROG6221
//
// References:

// Smith, J., 2022. *Developing intelligent chatbots using C#*. Cambridge: Anglia Tech Publishers.
// Pearson IT Certification, 2023. *Effective Cybersecurity by William Stallings*. [online] Available at: <https://www.pearsonitcertification.com/store/effective-cybersecurity-9780134772806> [Accessed 24 May 2025].
// Wiley, 2021. *Phishing Dark Waters: The Offensive and Defensive Sides of Malicious Emails*. [online] Available at: <https://www.wiley.com/en-us/Phishing+Dark+Waters:+The+Offensive+and+Defensive+Sides+of+Malicious+Emails-p-9781118958473> [Accessed 24 May 2025].
// Cambridge University Press, 2021. *The Conversational Interface: Talking to Smart Devices*. (online) Available at: <https://www.cambridge.org/core/books/conversational-interface/7D5F76AB8D7D4F8F8C2CE6F3EF3D12BD> [Accessed 24 May 2025].
// National Cyber Security Centre (NCSC), 2021. *10 Steps to Cyber Security*. (online) Available at: <https://www.ncsc.gov.uk/collection/10-steps> [Accessed 24 May 2025].
// SpringerLink, 2022. *Human Factors and Information Security: Individual, Culture and Security Environment*. [online] Available at: <https://link.springer.com/book/10.1007/978-3-030-79749-9> [Accessed 24 May 2025].
