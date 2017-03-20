namespace MvcDynamicForms.Demo.Models
{
    using System;
    using MvcDynamicForms.Core;
    using MvcDynamicForms.Core.Enums;
    using MvcDynamicForms.Core.Fields;
    using MvcDynamicForms.Core.Fields.Abstract;

    public static class FormProvider
    {
        public static Form GetForm()
        {
            /*
             * This method sets up the Form and Field objects that 
             * are needed to dynamically generate html forms at runtime.
             * 
             * Of course, there are other ways of going about defining your forms and their fields.
             * I used a static class in this demo application for simplicity.
             * In the real world, you could store your field definitions anywhere.
             * 
             * For example, you could create a database table to store all 
             * of the data needed to create the form fields below.
             * Some of your end users could have access to some kind of interface to create, update,
             * or delete the form field definitions in the database.
             * This described scenario was actually the inspiration for this project.
             * 
             * There are 7 different Field types that can be used to construct the form:
             *  - TextBox (single line text input)
             *  - Textarea (multi line text input)
             *  - Checkbox
             *  - CheckboxList
             *  - RadioList
             *  - Select (Drop down lists and List boxes)
             *  - Literal (Any custom html at all. For display purposes only (no user input))
             *  
             * Each Field type have a few things in common:
             *  - Title property: Used when storing end user's responses.
             *  - Prompt property: Question asked to the user for each field.
             *  - DisplayOrder property: The order that the field is displayed to the user.
             *  - Required property: Is the user required to complete the field?
             *  - InputHtmlAttributes: Allows the developer to set the input elements html attributes
             *  
             * There are other properties and behaviors that some Field types do not share with each other.
             * Take a look through the members of each Type to see what you can do.
             * Much of each type's unique functionality is demonstrated below.
             * Feel free to tinker around in this file, changing and adding fields.
             * Don't forget to add newly created fields to the Form.
             * 
             * The Form object is the object that contains all of your Field objects, 
             * triggers validation and rendering, and lets the developer access user responses.
             * When constructing your form, you can use Form.AddFields() to get your Fields
             * into the form (imagine that!).
             * 
             * Check out
             *    /Controllers/HomeController.cs
             *    /Views/Home/Demo.cshtml
             *    /Views/Home/Responses.cshtml
             * to learn how to use the Form object in your web application.
             */

            // create fields
            var description = new Literal
            {
                Key = "description",
                Template = String.Format("<p>{0}</p>", PlaceHolders.Literal),
                DisplayOrder = 10,
                Html =
                    "This is a dynamically generated form. All of the input fields on this form are generated at runtime."
            };

            var name = new TextBox
            {
                ResponseTitle = "Name",
                Prompt = "Enter your full name:",
                DisplayOrder = 20,
                Required = true,
                RequiredMessage = "Your full name is required",
            };

            var gender = new RadioList
            {
                DisplayOrder = 30,
                ResponseTitle = "Gender",
                Prompt = "Select your gender:",
                Required = true,
                Orientation = Orientation.Vertical
            };
            gender.AddChoices("Male,Female", ",");

            var email = new TextBox
            {
                DisplayOrder = 25,
                ResponseTitle = "Email Address",
                Prompt = "Enter your email address:",
                Required = true,
                RegexMessage = "Must be a valid email address",
                RegularExpression =
                    @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            };

            var sports = new CheckBoxList
            {
                DisplayOrder = 40,
                ResponseTitle = "Favorite Sports",
                Prompt = "What are your favorite sports?",
                Orientation = Orientation.Horizontal
            };
            sports.AddChoices("Baseball,Football,Soccer,Basketball,Tennis,Boxing,Golf", ",");

            var states = new Select
            {
                DisplayOrder = 50,
                ResponseTitle = "Visited States",
                MultipleSelection = true,
                Size = 10,
                Prompt = "What US states have you visited? (Use the ctrl key to select multiple states.)"
            };
            states.AddChoices(
                "Alabama,Alaska,Arizona,Arkansas,California,Colorado,Connecticut,Delaware,Florida,Georgia,Hawaii,Idaho,Illinois,Indiana,Iowa,Kansas,Kentucky,Louisiana,Maine,Maryland,Massachusetts,Michigan,Minnesota,Mississippi,Missouri,Montana,Nebraska,Nevada,New Hampshire,New Jersey,New Mexico,New York,North Carolina,North Dakota,Ohio,Oklahoma,Oregon,Pennsylvania,Rhode Island,South Carolina,South Dakota,Tennessee,Texas,Utah,Vermont,Virginia,Washington,West Virginia,Wisconsin,Wyoming",
                ",");

            var bio = new TextArea
            {
                DisplayOrder = 60,
                ResponseTitle = "Bio",
                Prompt = "Describe yourself:"
            };
            bio.InputHtmlAttributes.Add("cols", "40");
            bio.InputHtmlAttributes.Add("rows", "6");

            var month = new Select
            {
                DisplayOrder = 70,
                ResponseTitle = "Month Born",
                Prompt = "What month were you born in?",
                ShowEmptyOption = true,
                EmptyOption = "- Select One - "
            };
            month.AddChoices("January,February,March,April,May,June,July,August,September,October,November,December",
                ",");

            var agree = new CheckBox
            {
                DisplayOrder = 80,
                ResponseTitle = "Agrees To Terms",
                Prompt = "I agree to all of the terms in the EULA.",
                Required = true,
                RequiredMessage = "You must agree to the EULA!"
            };

            var eula = new Literal
            {
                DisplayOrder = 75,
                Html =
                    string.Format(@"<textarea readonly=""readonly"" rows=""8"" cols=""60"">{0}</textarea>", GetEULA())
            };

            var file = new FileUpload
            {
                Prompt = "Your photo",
                InvalidExtensionError = "Image files only.",
                ValidExtensions = ".jpg,.gif,.png",
                Required = true,
                DisplayOrder = 73
            };
            file.Validated += new ValidatedEventHandler(file_Validated);
            file.Posted += new FilePostedEventHandler(file_Posted);

            var hidden = new Hidden
            {
                ResponseTitle = "A Hidden Field",
                Value = "some value"
            };

            // create form and add fields to it
            var form = new Form();
            form.AddFields(description, name, gender, email, sports, states, bio, month, agree, eula, file, hidden);

            return form;
        }

        static void file_Posted(FileUpload fileUploadField, EventArgs e)
        {
            // here, you can do something with the posted file
            // (save it, email it, etc, or test it and report back to the user)
            // this event gets fired as soon as the dynamic form is model bound
        }

        static void file_Validated(InputField inputField, InputFieldValidationEventArgs e)
        {
            // here, you can also do something with the posted file
            // (save it, email it, etc, or test it and report back to the user)
            // this event gets fired following the validation of any class derived from InputField
            // here you can have more fine grained control of validation
            // for example:

            if (e.IsValid)
            {
                var fileUpload = inputField as FileUpload;

                if (fileUpload.PostedFile.ContentLength > 102400)
                {
                    fileUpload.Error = "The file is too large.";
                }
                else
                {
                    //fileUpload.PostedFile.SaveAs(fileUpload.PostedFile.FileName);
                }
            }
        }

        private static string GetEULA()
        {
            return @"License: GNU General Public License version 2 (GPLv2)
Copyright (C) 1989, 1991 Free Software Foundation, Inc.
59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

Preamble

The licenses for most software are designed to take away your freedom to share and change it. By contrast, the GNU General Public License is intended to guarantee your freedom to share and change free software--to make sure the software is free for all its users. This General Public License applies to most of the Free Software Foundation's software and to any other program whose authors commit to using it. (Some other Free Software Foundation software is covered by the GNU Library General Public License instead.) You can apply it to your programs, too.

When we speak of free software, we are referring to freedom, not price. Our General Public Licenses are designed to make sure that you have the freedom to distribute copies of free software (and charge for this service if you wish), that you receive source code or can get it if you want it, that you can change the software or use pieces of it in new free programs; and that you know you can do these things.

To protect your rights, we need to make restrictions that forbid anyone to deny you these rights or to ask you to surrender the rights. These restrictions translate to certain responsibilities for you if you distribute copies of the software, or if you modify it.

For example, if you distribute copies of such a program, whether gratis or for a fee, you must give the recipients all the rights that you have. You must make sure that they, too, receive or can get the source code. And you must show them these terms so they know their rights.

We protect your rights with two steps: (1) copyright the software, and (2) offer you this license which gives you legal permission to copy, distribute and/or modify the software.

Also, for each author's protection and ours, we want to make certain that everyone understands that there is no warranty for this free software. If the software is modified by someone else and passed on, we want its recipients to know that what they have is not the original, so that any problems introduced by others will not reflect on the original authors' reputations.

Finally, any free program is threatened constantly by software patents. We wish to avoid the danger that redistributors of a free program will individually obtain patent licenses, in effect making the program proprietary. To prevent this, we have made it clear that any patent must be licensed for everyone's free use or not licensed at all.

The precise terms and conditions for copying, distribution and modification follow.

TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION

0. This License applies to any program or other work which contains a notice placed by the copyright holder saying it may be distributed under the terms of this General Public License. The ""Program"", below, refers to any such program or work, and a ""work based on the Program"" means either the Program or any derivative work under copyright law: that is to say, a work containing the Program or a portion of it, either verbatim or with modifications and/or translated into another language. (Hereinafter, translation is included without limitation in the term ""modification"".) Each licensee is addressed as ""you"".

Activities other than copying, distribution and modification are not covered by this License; they are outside its scope. The act of running the Program is not restricted, and the output from the Program is covered only if its contents constitute a work based on the Program (independent of having been made by running the Program). Whether that is true depends on what the Program does.

1. You may copy and distribute verbatim copies of the Program's source code as you receive it, in any medium, provided that you conspicuously and appropriately publish on each copy an appropriate copyright notice and disclaimer of warranty; keep intact all the notices that refer to this License and to the absence of any warranty; and give any other recipients of the Program a copy of this License along with the Program.

You may charge a fee for the physical act of transferring a copy, and you may at your option offer warranty protection in exchange for a fee.

2. You may modify your copy or copies of the Program or any portion of it, thus forming a work based on the Program, and copy and distribute such modifications or work under the terms of Section 1 above, provided that you also meet all of these conditions:

a) You must cause the modified files to carry prominent notices stating that you changed the files and the date of any change.

b) You must cause any work that you distribute or publish, that in whole or in part contains or is derived from the Program or any part thereof, to be licensed as a whole at no charge to all third parties under the terms of this License.

c) If the modified program normally reads commands interactively when run, you must cause it, when started running for such interactive use in the most ordinary way, to print or display an announcement including an appropriate copyright notice and a notice that there is no warranty (or else, saying that you provide a warranty) and that users may redistribute the program under these conditions, and telling the user how to view a copy of this License. (Exception: if the Program itself is interactive but does not normally print such an announcement, your work based on the Program is not required to print an announcement.)

These requirements apply to the modified work as a whole. If identifiable sections of that work are not derived from the Program, and can be reasonably considered independent and separate works in themselves, then this License, and its terms, do not apply to those sections when you distribute them as separate works. But when you distribute the same sections as part of a whole which is a work based on the Program, the distribution of the whole must be on the terms of this License, whose permissions for other licensees extend to the entire whole, and thus to each and every part regardless of who wrote it.

Thus, it is not the intent of this section to claim rights or contest your rights to work written entirely by you; rather, the intent is to exercise the right to control the distribution of derivative or collective works based on the Program.

In addition, mere aggregation of another work not based on the Program with the Program (or with a work based on the Program) on a volume of a storage or distribution medium does not bring the other work under the scope of this License.

3. You may copy and distribute the Program (or a work based on it, under Section 2) in object code or executable form under the terms of Sections 1 and 2 above provided that you also do one of the following:

a) Accompany it with the complete corresponding machine-readable source code, which must be distributed under the terms of Sections 1 and 2 above on a medium customarily used for software interchange; or,

b) Accompany it with a written offer, valid for at least three years, to give any third party, for a charge no more than your cost of physically performing source distribution, a complete machine-readable copy of the corresponding source code, to be distributed under the terms of Sections 1 and 2 above on a medium customarily used for software interchange; or,

c) Accompany it with the information you received as to the offer to distribute corresponding source code. (This alternative is allowed only for noncommercial distribution and only if you received the program in object code or executable form with such an offer, in accord with Subsection b above.)

The source code for a work means the preferred form of the work for making modifications to it. For an executable work, complete source code means all the source code for all modules it contains, plus any associated interface definition files, plus the scripts used to control compilation and installation of the executable. However, as a special exception, the source code distributed need not include anything that is normally distributed (in either source or binary form) with the major components (compiler, kernel, and so on) of the operating system on which the executable runs, unless that component itself accompanies the executable.

If distribution of executable or object code is made by offering access to copy from a designated place, then offering equivalent access to copy the source code from the same place counts as distribution of the source code, even though third parties are not compelled to copy the source along with the object code.

4. You may not copy, modify, sublicense, or distribute the Program except as expressly provided under this License. Any attempt otherwise to copy, modify, sublicense or distribute the Program is void, and will automatically terminate your rights under this License. However, parties who have received copies, or rights, from you under this License will not have their licenses terminated so long as such parties remain in full compliance.

5. You are not required to accept this License, since you have not signed it. However, nothing else grants you permission to modify or distribute the Program or its derivative works. These actions are prohibited by law if you do not accept this License. Therefore, by modifying or distributing the Program (or any work based on the Program), you indicate your acceptance of this License to do so, and all its terms and conditions for copying, distributing or modifying the Program or works based on it.

6. Each time you redistribute the Program (or any work based on the Program), the recipient automatically receives a license from the original licensor to copy, distribute or modify the Program subject to these terms and conditions. You may not impose any further restrictions on the recipients' exercise of the rights granted herein. You are not responsible for enforcing compliance by third parties to this License.

7. If, as a consequence of a court judgment or allegation of patent infringement or for any other reason (not limited to patent issues), conditions are imposed on you (whether by court order, agreement or otherwise) that contradict the conditions of this License, they do not excuse you from the conditions of this License. If you cannot distribute so as to satisfy simultaneously your obligations under this License and any other pertinent obligations, then as a consequence you may not distribute the Program at all. For example, if a patent license would not permit royalty-free redistribution of the Program by all those who receive copies directly or indirectly through you, then the only way you could satisfy both it and this License would be to refrain entirely from distribution of the Program.

If any portion of this section is held invalid or unenforceable under any particular circumstance, the balance of the section is intended to apply and the section as a whole is intended to apply in other circumstances.

It is not the purpose of this section to induce you to infringe any patents or other property right claims or to contest validity of any such claims; this section has the sole purpose of protecting the integrity of the free software distribution system, which is implemented by public license practices. Many people have made generous contributions to the wide range of software distributed through that system in reliance on consistent application of that system; it is up to the author/donor to decide if he or she is willing to distribute software through any other system and a licensee cannot impose that choice.

This section is intended to make thoroughly clear what is believed to be a consequence of the rest of this License.

8. If the distribution and/or use of the Program is restricted in certain countries either by patents or by copyrighted interfaces, the original copyright holder who places the Program under this License may add an explicit geographical distribution limitation excluding those countries, so that distribution is permitted only in or among countries not thus excluded. In such case, this License incorporates the limitation as if written in the body of this License.

9. The Free Software Foundation may publish revised and/or new versions of the General Public License from time to time. Such new versions will be similar in spirit to the present version, but may differ in detail to address new problems or concerns.

Each version is given a distinguishing version number. If the Program specifies a version number of this License which applies to it and ""any later version"", you have the option of following the terms and conditions either of that version or of any later version published by the Free Software Foundation. If the Program does not specify a version number of this License, you may choose any version ever published by the Free Software Foundation.

10. If you wish to incorporate parts of the Program into other free programs whose distribution conditions are different, write to the author to ask for permission. For software which is copyrighted by the Free Software Foundation, write to the Free Software Foundation; we sometimes make exceptions for this. Our decision will be guided by the two goals of preserving the free status of all derivatives of our free software and of promoting the sharing and reuse of software generally.

NO WARRANTY

11. BECAUSE THE PROGRAM IS LICENSED FREE OF CHARGE, THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR OTHER PARTIES PROVIDE THE PROGRAM ""AS IS"" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU. SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING, REPAIR OR CORRECTION.

12. IN NO EVENT UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING WILL ANY COPYRIGHT HOLDER, OR ANY OTHER PARTY WHO MAY MODIFY AND/OR REDISTRIBUTE THE PROGRAM AS PERMITTED ABOVE, BE LIABLE TO YOU FOR DAMAGES, INCLUDING ANY GENERAL, SPECIAL, INCIDENTAL OR CONSEQUENTIAL DAMAGES ARISING OUT OF THE USE OR INABILITY TO USE THE PROGRAM (INCLUDING BUT NOT LIMITED TO LOSS OF DATA OR DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY YOU OR THIRD PARTIES OR A FAILURE OF THE PROGRAM TO OPERATE WITH ANY OTHER PROGRAMS), EVEN IF SUCH HOLDER OR OTHER PARTY HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.";
        }
    }
}