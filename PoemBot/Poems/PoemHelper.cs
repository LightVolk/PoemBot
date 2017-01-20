using PoemBot.Poems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PoemBot
{
    public class PoemHelper
    {
        public PoemHelper()
        { }

        /// <summary>
        /// Получить стихотворение
        /// </summary>
        /// <param name="poemLines"></param>
        /// <returns></returns>
        public Poem MakePoem(List<String> poemLines)
        {
            Poem poem = new Poem();
            bool catchHashId = false;
            bool catchTitle = false;
            bool catchLines = false;
            bool catchUrl = false;
            bool catchHits = false;
            bool catchLikes = false;
           

            foreach(var line in poemLines)
            {
                if(line.Equals("hashid"))
                {
                    catchHashId = true;
                    continue;
                }
                else if(line.Equals("lines"))
                {
                    catchLines = true;
                    break;
                }
                else if(line.Equals("title"))
                {
                    catchLines = true;
                    break;
                }
                else if(line.Equals("url"))
                {
                    catchUrl = true;
                    continue;
                }
                else if(line.Equals("hits"))
                {
                    catchHits = true;
                    continue;
                }
                else if(line.Equals("liked"))
                {
                    catchLikes = true;
                    continue;
                }
                else //начинаем искать нужные строки
                {
                    if(catchHashId)
                    {
                        poem.HashId = line;
                        catchHashId = false;//поймали id и хватит
                    }
                    else if(catchTitle)
                    {
                        poem.Title = line;
                        catchTitle =false;
                    }
                    else if(catchLines&&catchHits==false)
                    {
                        poem.Lines.Add(line);
                    }
                    else if(catchHits)
                    {
                        poem.Hits = Convert.ToInt32(line);
                        catchHits = false;
                    }
                    else if(catchLikes)
                    {
                        poem.Likes =Convert.ToInt32(line);
                        catchLikes = false;
                    }

                }
            }

            return poem;
        }


        public String MakePoemReply(List<String> poemLines)
        {
            try
            {
                Poem poem = new Poem();
                bool catchHashId = false;
                bool catchTitle = false;
                bool catchLines = false;
                bool catchUrl = false;
                bool catchHits = false;
                bool catchLikes = false;


                foreach (var line in poemLines)
                {
                    if (line == null)
                        continue;

                    if (line.Equals("hashid"))
                    {
                        catchHashId = true;
                        continue;
                    }
                    else if (line.Equals("lines"))
                    {
                        catchLines = true;
                        continue;
                    }
                    else if (line.Equals("title"))
                    {
                        catchTitle = true;
                        continue;
                    }
                    else if (line.Equals("url"))
                    {
                        catchUrl = true;
                        continue;
                    }
                    else if (line.Equals("hits"))
                    {
                        catchHits = true;
                        continue;
                    }
                    else if (line.Equals("liked"))
                    {
                        catchLikes = true;
                        continue;
                    }
                    else //начинаем искать нужные строки
                    {
                        if (catchHashId)
                        {
                            poem.HashId = line;
                            catchHashId = false;//поймали id и хватит
                        }
                        else if (catchTitle)
                        {
                            poem.Title = line;
                            catchTitle = false;
                        }
                        else if (catchLines && catchUrl == false)
                        {                                                       
                                poem.Lines.Add(line);                            
                        }
                        else if(catchUrl)
                        {
                            poem.Url = line;
                            catchUrl = true;
                            catchLines = false;
                        }
                        else if (catchHits)
                        {
                            poem.Hits = Convert.ToInt32(line);
                            catchHits = false;
                            catchLines = false;
                        }
                        else if (catchLikes)
                        {
                            poem.Likes = Convert.ToInt32(line);
                            catchLikes = false;
                        }

                    }
                }

                List<string> resultColl = new List<string>();
                resultColl.Add("\t\tTitle:");
                resultColl.Add("\n");
                resultColl.Add(poem.Title);
                foreach (var line in poem.Lines)
                {
                    resultColl.Add(line);
                }
                resultColl.Add("\n");
                resultColl.Add("\n");

                resultColl.Add(String.Format("Url: {0}", poem.Url));
                resultColl.Add("\n");
                resultColl.Add(String.Format("Hits: {0}", poem.Hits));
                resultColl.Add("\n");
                resultColl.Add(String.Format("Likes: {0}", poem.Likes));

                StringBuilder resultStr = new StringBuilder();
                foreach (var result in resultColl)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        resultStr.AppendLine("\n");
                        resultStr.AppendLine("\n");
                    }
                    else
                    {
                        resultStr.AppendLine(result);
                        resultStr.AppendLine("\n");
                    }
                }
                return resultStr.ToString();
            }
            catch(Exception ex)
            {
                Console.Write("{0} {1}", ex.Message, ex.StackTrace);
            }
            return String.Empty;
        }
    }
}